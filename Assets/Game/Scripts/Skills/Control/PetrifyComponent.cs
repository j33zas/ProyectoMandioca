using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PetrifyComponent : MonoBehaviour
{
    MinionLifeSystem minionlifesystem;
    Action<Vector3> listener;

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
        listener.Invoke(minionlifesystem.gameObject.transform.position);
    }

    public void Configure(Action<Vector3> _listener)
    {
        listener = _listener;
    }
}
