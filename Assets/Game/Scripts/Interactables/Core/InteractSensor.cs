using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using System;

public class InteractSensor : MonoBehaviour
{
    Interactable most_close;

    bool isclose;

    bool canrecolect;
    [Range(0f,0.5f)]
    public float cooldownrecolect;
    float timer;

    bool interact;

    Interactable[] interactables;
    Interactable current;

    public WalkingEntity collector;

    private void Awake()
    {
        canrecolect = true;

        newitem = true;
    }

    public void Interact()
    {
        if (isclose && most_close != null && interact)
        {
            most_close.Execute(collector);
            interact = false;
        }
    }
    public void Disapear()
    {
        WorldItemInfo.instance.Hide();
        most_close.Exit();
    }

    bool newitem;

    private void Update()
    {
        interactables = FindObjectsOfType<Interactable>();
        current = interactables.ReturnMostClose(transform.position);

        if (most_close == null)
        {
            most_close = current;
        }

        if (!most_close) return;

        if (current != most_close)
        {
            most_close.Exit();
            most_close = current;
            newitem = true;
        }

        if (Vector3.Distance(most_close.transform.position, transform.position) < most_close.distancetoInteract)
        {
            isclose = true;

            if (newitem)
            {
                most_close.ShowInfo(collector);
                newitem = false;
            }

            if (most_close.autoexecute || Input.GetButton("XBOX360_Square"))
            {
                if (interact)
                {
                    if (canrecolect)
                    {
                        canrecolect = false;
                        most_close.Execute(collector);
                        interact = false;
                    }
                }
                else
                {
                    canrecolect = false;
                    interact = true;
                }

                newitem = true;
            }
        }
        else
        {
            isclose = false;
            most_close.Exit();
            WorldItemInfo.instance.Hide();
            newitem = true;
        }

        if (!canrecolect) {
            if (timer < cooldownrecolect) timer = timer + 1 * Time.deltaTime;
            else { canrecolect = true; timer = 0; }
        }
    }
 
}
