using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//esta un poco hardodeado... 
//aca podria genera una herencia para las barras de torta... pero bueno, no alcanzó el tiempo
//luego lo hago


    //lo importante es...

    //pivotPointScale este hay que asegurarse que se escale de 0 a 1? o 0 a 100? hay que probar
    //El Configure que es para reconfigurar el maximo y minimo... con esto ya queda guardado
    //El SetValue aca le pasas el valor y hace toda su magia
public class GenericBar : MonoBehaviour
{
    public Transform pivotPointToScale;

    public Image spcake;

    int minValue;
    int maxValue;
    float scaler;

    public enum axis { x, y, z }
    public axis _axis;

    public Text porcentaje;

    public bool realvalue;

    Vector3 scale = new Vector3(1, 1, 1);

    public void Configure(int minValue, int maxValue, float scaler)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.scaler = scaler;
    }

    public void SetValue(int value)
    {
        var percent = (value * 100) / maxValue;
        if (porcentaje) porcentaje.text = !realvalue ? ((int)percent).ToString() + "%" : value + " / " + maxValue;
        if (_axis == axis.x) scale.x = percent * scaler;
        if (_axis == axis.y) scale.y = percent * scaler;
        if (_axis == axis.z) scale.z = percent * scaler;
        pivotPointToScale.localScale = scale;
    }
    public void SetValue(float value)
    {
        var percent = (value * 100) / maxValue;
        if (porcentaje) porcentaje.text = !realvalue ? ((int)percent).ToString() + "%" : value + " / " + maxValue;
        if (_axis == axis.x) scale.x = percent * scaler;
        if (_axis == axis.y) scale.y = percent * scaler;
        if (_axis == axis.z) scale.z = percent * scaler;
        pivotPointToScale.localScale = scale;
    }
    public void SetValueCake(float value)
    {
        var percent = (value * 100) / maxValue;
        if (porcentaje) porcentaje.text = !realvalue ? ((int)percent).ToString() + "%" : value + " / " + maxValue;
        spcake.fillAmount = percent * scaler;
    }
}
