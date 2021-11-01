using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellGrid : MonoBehaviour
{

    [SerializeField]
    RangeVariable _rows;

    [SerializeField]
    RangeVariable _columns;

    [SerializeField]
    private GameObject _cellPrefab;

    [SerializeField]
    private float _simulationInterval = 1;

    [SerializeField]
    private Button _btnTogglePlay;

    [SerializeField]
    private TextMeshProUGUI _loopIndicator;

    private float _nextSimTime = 1;

    private float _highestAxis;

    private GameObject[,] _grid;

    private GameObject go_Grid;

    [SerializeField]
    private Camera _camera;

    private bool _simStarted;

    private int loops = 0;

    private void Awake()
    {



    }
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            togglePlay();
        }

        SimLoop(_simStarted);
    }

    public void togglePlay()
    {
        _simStarted = !_simStarted;

        _btnTogglePlay.GetComponentInChildren<TextMeshProUGUI>().SetText((_simStarted) ? "PAUSE" : "PLAY");

    }

    public void togglePlay(bool _IsSimStarted)
    {
        _simStarted = _IsSimStarted;
        _btnTogglePlay.GetComponentInChildren<TextMeshProUGUI>().SetText((_simStarted) ? "PAUSE" : "PLAY");
    }

    public void initGame()
    {
        EmptyGrid();

        SetCamera();


        loops = 0;


        GenerateGrid();
    }

    public void EmptyGrid()
    {
        Debug.Log(go_Grid);

        if(go_Grid != null && go_Grid.transform.childCount > 0)
        {
            go_Grid.transform.position = Vector2.zero;

            foreach (Transform child in go_Grid.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

    }

    private void SimLoop(bool _startSim)
    {
        if (Time.time >= _nextSimTime && _startSim)
        {
            
            Debug.Log($"Simulation is {(_simStarted ? "started" : "stopped")}");

            loops++;

            _loopIndicator.SetText($"STEPS: {loops}");

            LoopThroughEachCell("countNeighborsAlive");

            LoopThroughEachCell("setCellState");

            _nextSimTime = Time.time + _simulationInterval;
        }
    }

    private void LoopThroughEachCell(string action)
    {

        Debug.Log(action);

        for (int indexRow = 0; indexRow < _rows.Value; indexRow++)
        {

            for (int indexColumn = 0; indexColumn < _columns.Value; indexColumn++)
            {
                switch (action)
                {
                    case "countNeighborsAlive":
                        CheckNeighbors(indexColumn, indexRow);
                        break;

                    case "setCellState":
                        SimulationStep(indexColumn, indexRow);
                        break;
                }

            }
        }
    }

    //Check if coords exist in grid
    private bool isInGrid(int indexColumn, int indexRow)
    {
        if( (indexColumn >= 0 && indexColumn < _columns.Value)  &&  (indexRow >= 0 && indexRow < _rows.Value) )
        {
            //Debug.Log($"_grid[{indexCol},{indexRow}] exists");
            //_grid[indexCol, indexRow].GetComponent<Cell>().toggleTested("neighbor");
            return true;
        }
        else
        {
            //Debug.Log($"_grid[{indexCol},{indexRow}] does not exist");
            return false;
        }
    }

    private void CheckNeighbors(int indexColumn, int indexRow)
    {
        Debug.Log($"Checking neighbors of _grid[{indexColumn},{indexRow}]");

        //_grid[indexColumn, indexRow].GetComponent<Cell>().toggleTested("itself");
        int neighborsAlive = 0;

        for (int j = indexRow - 1; j <= indexRow + 1; j++)
        {
            for (int i = indexColumn - 1; i <= indexColumn + 1; i++)
            {
                //If it's not the cell itself
                if(i != indexColumn || j != indexRow)
                {
                    //Check if coords are in grid
                    if(isInGrid(i, j))
                    {
                        //Check if neighbor (cell at these coords) is alive
                        if(_grid[i, j].GetComponent<Cell>().IsAlive)
                        {
                            neighborsAlive++;
                        }
                    }
                }
            }
        }

        //Debug.Log($"_grid[{indexCol},{indexRow}] has {neighborsAlive} neighbors alive");

        _grid[indexColumn, indexRow].GetComponent<Cell>().neighborsAlive = neighborsAlive;
        //_grid[indexCol, indexRow].GetComponentInChildren<TextMeshPro>().SetText($"{neighborsAlive}");
    }

    private void SimulationStep(int indexColumn, int indexRow)
    {
        Cell cell = _grid[indexColumn, indexRow].GetComponent<Cell>();

        if (cell.neighborsAlive == 3)
        {
            cell.IsAlive = true;
        }
        else if(cell.neighborsAlive == 2)
        {

        }
        else
        {
            cell.IsAlive = false;
        }

        cell.toggleMaterial(cell.IsAlive);
    }


    private void GenerateGrid()
    {
        go_Grid = gameObject;
        _grid = new GameObject[_columns.Value, _rows.Value];


        for (int j = 0; j < _rows.Value; j++)
        {
            for (int i = 0; i < _columns.Value; i++)
            {
                generateCell(_cellPrefab, i, j);
                //_grid[x,y] = generateCell(_cellPrefab, new Vector2(x, y));
            }
        }

        CenterGrid();   
    }


    private void CenterGrid()
    {
        go_Grid.transform.position = new Vector2(_columns.Value / -2f + .5f, _rows.Value / 2f - .5f);
    }

    private void generateCell(GameObject cellPrefab, int i, int j)
    {
        Vector2 cellPosition = new Vector2(i, -j);
        _grid[i, j] = Instantiate(cellPrefab, cellPosition, Quaternion.identity, go_Grid.transform);
    }

    private void SetCamera()
    {
        _camera.orthographicSize = .7f * Mathf.Max(_rows.Value, _columns.Value / 16f / 9f);
    }
}
