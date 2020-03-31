using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptRandom : MonoBehaviour
{
    float pepe;

    private void Awake()
    {
       // DevelopTools.CreateSlider(pepe, min, max, CambiarPepe);
    }

    //otro programador
    public string CambiarPepe(float val)
    {
        pepe = val;
        return "Pepe ahora vale: " + pepe;
    }
}
    
