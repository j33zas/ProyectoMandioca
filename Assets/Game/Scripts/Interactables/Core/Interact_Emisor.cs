using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Emisor : Interactable
{
    public Interact_Receptor[] results;

    public override void Execute(WalkingEntity entity)
    {
        foreach (var r in results)
        {
            r.Execute();
        }
    }

    public override void Exit() => WorldItemInfo.instance.Hide();

    public override void ShowInfo(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position,
                                    "Boton",
                                    "",
                                    "pulsar");
    }

    private void OnDrawGizmos()
    {
        foreach (var r in results)
        {
            Gizmos.DrawLine(this.transform.position, r.transform.position);
        }
    }
}
