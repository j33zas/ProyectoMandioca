﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class Interactable : MonoBehaviour
{
    public float distancetoInteract = 1f;

    public Transform pointToMessage;

    public FeedbackInteractBase feedback;
    public FeedbackInteractBase feedback2;

    public abstract void ShowInfo(WalkingEntity entity);
    public bool autoexecute;
    public abstract void Execute(WalkingEntity collector);
    public abstract void Exit();

}