using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PetrifyComponent : MonoBehaviour
{
    MinionLifeSystem minionlifesystem;
    Action<Vector3, PetrifyComponent> listener;

    private void Awake()
    {
        minionlifesystem = GetComponent<MinionLifeSystem>();
    }

    public void OnBegin()
    {
        if(minionlifesystem)
        minionlifesystem.AddEventOnDeath(OnExecute);
    }
    public void OnEnd()
    {
        if (minionlifesystem)
            minionlifesystem.RemoveEventOnDeath(OnExecute);
    }

    public void OnExecute()
    {
        if (minionlifesystem)
            listener.Invoke(minionlifesystem.gameObject.transform.position, this);
    }

    public void Configure(Action<Vector3,PetrifyComponent> _listener)
    {
        if (minionlifesystem)
            listener = _listener;
    }
}
