using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUIBases : Interactable
{
    public string title;
    public string info;

    public int id_UI_Base;

    public UI_Base ui_to_open;

    private void Start()
    {
        var bases = FindObjectsOfType<UI_Base>();

        foreach (var v in bases)
        {
            if (v.idfinder == id_UI_Base)
            {
                ui_to_open = v;
            }
        }
    }


    public override void Execute(WalkingEntity entity)
    {
        ui_to_open.Open();
    }

    public override void Exit()
    {
        ui_to_open.Close();
    }

    public override void ShowInfo(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(transform.position, title, info, "", true);
    }
}
