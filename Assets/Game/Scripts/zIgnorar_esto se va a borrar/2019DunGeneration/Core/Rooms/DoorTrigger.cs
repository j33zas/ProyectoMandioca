using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public event Action<GameObject> callBack;

    public Sensor sensor;

    public void AddEventListener(Action<GameObject> _callback) => callBack += _callback;
    public void Initialize() => sensor.AddCallback_OnTriggerEnter(OnTriggerEnterSensor);
    void OnTriggerEnterSensor(GameObject go) => callBack(go);
}
