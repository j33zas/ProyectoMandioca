using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using System;

public class InteractSensor : MonoBehaviour
{
    public Interactable most_close;

    public bool isclose;
    public bool canrecolect;

    public bool optimize;

    [Range(0f,0.5f)]
    public float cooldownrecolect;
    float timer;

    Interactable[] interactables;
    Interactable current;

    public WalkingEntity collector;

    public bool canInteract;

    private void Awake()
    {
        canrecolect = true;
        newitem = true;
    }

    public void OnInteractDown()
    {
        canInteract = true;

        if (isclose && most_close != null)
        {
            most_close.Execute(collector);
            optimize = true;
        }
    }
    public void OnInteractUp()
    {
        canInteract = false;

        if (most_close != null)
        {
            most_close.Exit();
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
        interactables = FindObjectsOfType<Interactable>();//hay que optimizar esto, es muy pesado un findobject en un Update

        current = interactables.ReturnMostClose(transform.position);//esto tambien se puede optimizar mas a delante con un return most close que busque por grupos

        if (most_close == null) 
        { 
            most_close = current;
            return; 
        }

        if (current != most_close)
        {
            most_close.Exit();
            most_close = current;
            newitem = true;
        }

        if (I_Have_Good_Distace_To_Interact())
        {
            isclose = true;

            if (newitem)
            {
                most_close.ShowInfo(collector);
                newitem = false;
            }

            if (most_close.autoexecute || canInteract)
            {
                canrecolect = false;
                most_close.Execute(collector);
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


    bool I_Have_Good_Distace_To_Interact() { return Vector3.Distance(most_close.transform.position, transform.position) < most_close.distancetoInteract; }

    
}
