using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DevelopTools;

public class CharacterAnimEvent : MonoBehaviour
{
    EventManager myeventManager;

    private void Awake()
    {
        myeventManager = new EventManager();
    }

    public void Add_Callback(string s, EventManager.EventReceiver receiver)
    {
        myeventManager.SubscribeToEvent(s, receiver);
    }

    public void Add_Callback(string s, EventManager.EventReceiverParam receiver)
    {
        myeventManager.SubscribeToEvent(s, receiver);
    }

    public void EVENT_Callback(string s)
    {
        myeventManager.TriggerEvent(s);
    }

}
