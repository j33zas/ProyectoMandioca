using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable Setup")]
    public float distancetoInteract = 1f;
    public bool autoexecute;
    public Transform pointToMessage;
    public FeedbackInteractBase[] feedback;

    public abstract void ShowInfo(WalkingEntity entity);
    
    public abstract void Execute(WalkingEntity collector);
    public abstract void Exit();

}
