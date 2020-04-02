using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class OnlyMessage : Interactable
{
    [Multiline(5)]
    public string message;
    public string tiitle;

    private void Awake()
    {
        //lookat.SetSource(0, Camera.main.gameObject.GetComponent<ConstraintSource>());
    }

    public override void Execute(WalkingEntity entity)
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void ShowInfo(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, tiitle, message, "", true);
    }
    
}
