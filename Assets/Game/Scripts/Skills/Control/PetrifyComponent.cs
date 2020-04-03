using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PetrifyComponent : MonoBehaviour
{
    EnemyLifeBar EnemyLifeBar;
    Action<Vector3> listener;

    private void Awake()
    {
        EnemyLifeBar = GetComponent<EnemyLifeBar>();
    }

    public void OnBegin()
    {
        EnemyLifeBar.AddEventOnDeath(OnExecute);
    }
    public void OnEnd()
    {
        EnemyLifeBar.RemoveEventOnDeath(OnExecute);
    }

    public void OnExecute()
    {
        listener.Invoke(EnemyLifeBar.gameObject.transform.position);
    }

    public void Configure(Action<Vector3> _listener)
    {
        listener = _listener;
    }
}
