﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreamsBar : FrontendStatBase
{
    public Text val;
    public GenericBar genbar;

    public override void OnValueChange(int value, int max, bool anim = false)
    {
        genbar.Configure(max, 0.01f);
        genbar.SetValue(value);
    }
}
