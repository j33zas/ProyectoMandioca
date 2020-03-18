using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : FrontendStatBase
{
    public Text val;
    public GenericBar genbar;

    public override void OnLifeChange(int value, int max = 100, bool anim = false)
    {
        genbar.Configure(0, max, 0.01f);
        genbar.SetValue(value);
    }
}
