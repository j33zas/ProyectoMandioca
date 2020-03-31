using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomSliderComponentUI : MonoBehaviour
{
    Func<float, string> funcion;

    Text txt;

    public void ChangeValues(float v)
    {
        txt.text = funcion(v);
    }

    public void Configurate(Func<float, string> _f)
    {
        funcion = _f;
    }
}


