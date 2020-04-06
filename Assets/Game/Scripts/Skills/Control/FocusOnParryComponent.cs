using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FocusOnParryComponent : MonoBehaviour
{
    public CharacterHead charHead;
    Action<Vector3, FocusOnParryComponent> listener;


    public void OnBegin()
    {
        charHead.AddParry(OnExecute);
    }
    public void OnEnd()
    {
        charHead.RemoveParry(OnExecute);
    }

    public void OnExecute()
    {
       listener.Invoke(charHead.gameObject.transform.position, this);
    }

    public void Configure(Action<Vector3, FocusOnParryComponent> _listener)
    {
        listener = _listener;
    }
}
