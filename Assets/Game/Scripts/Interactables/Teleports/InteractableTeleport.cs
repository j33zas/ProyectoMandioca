using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InteractableTeleport : Interactable
{
    public string title = "Teleport";
    [Multiline(10)]
    public string info;

    public bool show = true;

    public Transform go_to;

    [Space(10)]
    [Header("Manualmente con mensaje personalizado")]
    public bool manual;


    public override void Execute(WalkingEntity entity)
    {
        Main.instance.GetChar().transform.position = go_to.position;
    }

    public override void Exit()
    {
        WorldItemInfo.instance.Hide();
    }

    public override void ShowInfo(WalkingEntity entity)
    {
        if (show)
        {
            if(pointToMessage != null) WorldItemInfo.instance.Show(pointToMessage.transform.position, title, info, "Entrar");
            else WorldItemInfo.instance.Show(this.transform.position, title, info, "Entrar");
        }
    }
}
