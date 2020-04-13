using System;
using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    public LayerMask layers;
    event Action<GameObject> Ev_Colision;
    event Action<GameObject> Ev_Exit;

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
    public Sensor SubscribeExitAction(Action<GameObject> ac) { Ev_Exit+= ac; return this; }
    public Sensor UnSubscribeAction(Action<GameObject> ac) { Ev_Colision -= ac; return this; }
    public Sensor UnSubscribeExitAction(Action<GameObject> ac) { Ev_Exit -= ac; return this; }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if ((1 << col.gameObject.layer & layers) != 0)
        {
            Ev_Colision(col.gameObject);
            //myc.enabled = false;
        }
    }
    protected virtual void OnTriggerExit(Collider col)
    {
        if ((1 << col.gameObject.layer & layers) != 0)
        {
            Ev_Exit(col.gameObject);
            //myc.enabled = false;
        }
    }
}