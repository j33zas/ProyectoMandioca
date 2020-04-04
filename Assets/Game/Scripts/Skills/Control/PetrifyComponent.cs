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
        minionlifesystem.AddEventOnDeath(OnExecute);
    }
    public void OnEnd()
    {
        minionlifesystem.RemoveEventOnDeath(OnExecute);
    }

    public void OnExecute()
    {
        listener.Invoke(minionlifesystem.gameObject.transform.position, this);
    }

    public void Configure(Action<Vector3,PetrifyComponent> _listener)
    {
        listener = _listener;
    }
}
