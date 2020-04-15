using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackInteract_Rotator : FeedbackInteractBase
{
    public Transform torot;
    Quaternion startrot = new Quaternion();
    public Vector3 cant_to_rotate = new Vector3(0,1,0);
    private void Start()
    {
        startrot = torot.rotation;
    }
    protected override void OnShow() {  }
    protected override void OnHide() { torot.rotation = startrot; }
    protected override void OnUpdate() 
    {
        torot.Rotate(cant_to_rotate);
    }
}
