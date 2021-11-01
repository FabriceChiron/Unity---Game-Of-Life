using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageSlider : MonoBehaviour
{

    [SerializeField]
    Slider _slider;

    [SerializeField]
    RangeVariable _sliderValue;

    private TextMeshProUGUI _valueFieldText;

    private void Awake()
    {
        Debug.Log(name);

        _valueFieldText = _slider.transform.parent.Find("ValueField").GetComponent<TextMeshProUGUI>();
        _slider.value = _sliderValue.Value;

        ValueChangeCheck();

    }

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ValueChangeCheck()
    {
        _sliderValue.Value = (int)_slider.value;
        _valueFieldText.SetText($"{_slider.value}");
    }


}
