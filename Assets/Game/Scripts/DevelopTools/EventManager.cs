using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DevelopTools
{
    public class EventManager
    {
        public delegate void EventReceiver(params object[] parameterContainer);
        private Dictionary<string, EventReceiver> events;


        public void SubscribeToEvent(string eventType, EventReceiver listener)
        {
            if (events == null)
                events = new Dictionary<string, EventReceiver>();

            if (!events.ContainsKey(eventType))
                events.Add(eventType, null);

            events[eventType] += listener;
        }

        public void UnsubscribeToEvent(string eventType, EventReceiver listener)
        {
            if (events != null)
            {
                if (events.ContainsKey(eventType))
                    events[eventType] -= listener;
            }
        }

        public void TriggerEvent(string eventType)
        {
            TriggerEvent(eventType, null);
        }

        public void TriggerEvent(string eventType, params object[] parametersWrapper)
        {
            if (events == null)
            {
                Debug.LogWarning("No events subscribed");
                return;
            }

            if (events.ContainsKey(eventType))
            {
                if (events[eventType] != null)
                    events[eventType](parametersWrapper);
            }
        }
    }
}
