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

    [SerializeField]
    RangeDoubleVariable _sliderDoubleValue;

    private TextMeshProUGUI _valueFieldText;

    private void Awake()
    {
        Debug.Log($"{name}");
        Debug.Log($"{_sliderDoubleValue}");
        _valueFieldText = _slider.transform.parent.Find("ValueField").GetComponent<TextMeshProUGUI>();
        _slider.value = (float) ((_sliderDoubleValue != null) ? _sliderDoubleValue.Value : _sliderValue.Value);
        _slider.value = Mathf.Round(_slider.value * 4) / 4;


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
        if(_sliderDoubleValue)
        {
            _slider.value = Mathf.Round(_slider.value * 4) / 4;
            _sliderDoubleValue.Value = _slider.value;
            _valueFieldText.SetText($"{_slider.value}s");
        }
        else
        {
            _sliderValue.Value = (int)_slider.value;
            _valueFieldText.SetText($"{_slider.value}");
        }
        
        
    }


}
