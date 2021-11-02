using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{

    [SerializeField]
    private Material _materialDead;
    [SerializeField]
    private Material _materialAlive;
    [SerializeField]
    private Material _materialIsTested;
    [SerializeField]
    private Material _materialIsNeighbor;

    private Renderer _renderer;

    private bool _isAlive;

    public bool IsAlive { get => _isAlive; set => _isAlive = value; }

    public int neighborsAlive;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(GetComponent<MeshRenderer>().materials[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {

        _isAlive = !_isAlive;

        toggleMaterial(_isAlive);

    }

    public void OnMouseEnter()
    {
        _renderer.material = _materialIsTested;
    }

    public void OnMouseExit()
    {
        toggleMaterial(IsAlive);
    }

    public void toggleMaterial(bool alive)
    {
        _renderer.material = (alive) ? _materialAlive : _materialDead;
        //GetComponent<MeshRenderer>().materials[0] = (alive) ? _materialAlive : _materialDead;
    }

    public void toggleTested(string test)
    {
        _renderer.material = (test == "neighbor") ? _materialIsNeighbor : _materialIsTested;
    }
}
