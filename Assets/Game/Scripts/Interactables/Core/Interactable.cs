using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float distancetoInteract = 1f;

    public Transform pointToMessage;

    public FeedbackInteractBase feedback;

    public abstract void ShowInfo(WalkingEntity entity);
    public bool autoexecute;
    public abstract void Execute(WalkingEntity collector);
    public abstract void Exit();

}
