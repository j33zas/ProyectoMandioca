using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FocusOnParryComponent : MonoBehaviour
{
    Action<Vector3, FocusOnParryComponent> listener;

    public void OnBegin()
    {
        foreach (var item in Main.instance.GetEnemies())
        {
            item.OnParried += OnExecute;
        }
    }
    public void OnEnd()
    {
        foreach (var item in Main.instance.GetEnemies())
        {
            item.OnParried -= OnExecute;
        }
    }

    public void OnExecute()
    {
       listener.Invoke(Main.instance.GetChar().gameObject.transform.position, this);
    }

    public void Configure(Action<Vector3, FocusOnParryComponent> _listener)
    {
        listener = _listener;
    }
}
