using System;
using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    public LayerMask layers;
    event Action<GameObject> Ev_Colision;

    public Collider myc;

    public void On() { myc.enabled = true; }
    public void Off() { myc.enabled = false; }

    public Sensor Initialize()
    {
        myc = GetComponent<Collider>();
        myc.isTrigger = true;
        return this;
    }

    public Sensor Configure(LayerMask layermask)
    {
        layers = layermask;
        return this;
    }

    public Sensor SubscribeAction(Action<GameObject> ac) { Ev_Colision += ac; return this; }

    private void OnTriggerEnter(Collider col)
    {
        if ((1 << col.gameObject.layer & layers) != 0)
        {
            Ev_Colision(col.gameObject);
        }
    }
}