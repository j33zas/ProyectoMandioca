using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAnimEvent : MonoBehaviour
{
    public Action rompecoco;
    public void AddEvent_RompeCoco(Action _r) => rompecoco += _r;
    public void EVENT_RompeCoco() => rompecoco.Invoke();


    
}
