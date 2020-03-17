using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : FrontendLifeBase
{
    public Text val;

    public override void OnLifeChange(int value, int max = 100, bool anim = false)
    {
        if (value >= max)
        {
            value = max;
        }

        //val * 100 / max
        val.text = value + "/" + max;

        int centconverted = (value * 100) / max;
        //aca lo convierto al porcentaje del max que me pasaron
        transform.localScale = new Vector3(centconverted * 0.01f, 1, 1);
    }
}
