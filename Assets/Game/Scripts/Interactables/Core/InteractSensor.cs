using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using System;

public class InteractSensor : MonoBehaviour
{
    public bool isclose;

    Interactable[] interactables;
    Interactable current;
    Interactable most_close;

    [Header("Walking Entity")]
    [SerializeField] WalkingEntity collector;

    

    private void Awake()
    {
        cooldown_to_next_recollection = true;
        can_show_info = true;
    }

    public void OnInteractDown()
    {
        if (isclose && most_close != null)
        {
            most_close.Execute(collector);
        }

        calculate_fast_recollection = true;
        timerfastrec = 0;
    }
    public void OnInteractUp()
    {
        if (most_close != null)
        {
            most_close.Exit();
        }
        Debug.Log("DISABLE");
        calculate_fast_recollection = false;
        timerfastrec = 0;
    }

    public void Disapear()
    {
        WorldItemInfo.instance.Hide();
        most_close.Exit();
    }

    bool can_show_info;

    private void Update()
    {
        isclose = false;
        Update_CheckFastRecollection();
        Update_CooldownNextRecollection();

        //buscamos los interactuables
        interactables = FindObjectsOfType<Interactable>();//hay que optimizar esto, es muy pesado un findobject en un Update
        if (interactables.Length == 0) return;
        if (interactables.Length == 1) current = interactables[0];
        else current = interactables.ReturnMostClose(transform.position);//esto tambien se puede optimizar mas a delante con un return most close que busque por grupos

        //si no hay un most_close previo le asigno uno
        if (most_close == null)
        {
            most_close = current;
            return;
        }

        //si lo hay, pero mi current es mas cercano, le asigno current
        if (current != most_close)
        {
            most_close.Exit();
            most_close = current;
            can_show_info = true;
        }

        //hasta ahora ya estariamos al dia de cual es el que tengo que calcular
        //ahora...

        
        if (I_Have_Good_Distace_To_Interact())
        {
            isclose = true;

            if (can_show_info)
            {
                most_close.ShowInfo(collector);
                can_show_info = false;
            }

            if (!cooldown_to_next_recollection)
            {
                if (most_close.autoexecute || can_fast_recollecion)
                {
                    cooldown_to_next_recollection = true;
                    most_close.Execute(collector);
                    can_show_info = true;
                    WorldItemInfo.instance.Hide();
                }
            } 
        }
        else
        {
            isclose = false;
            most_close.Exit();
            WorldItemInfo.instance.Hide();
            can_show_info = true;
        }
    }


    [Header("Fast Recollection")]
    [Range(0f, 1f)]
    public float time_to_fast_recollection = 0.5f;
    bool calculate_fast_recollection = false;
    [SerializeField] bool can_fast_recollecion = false;
    [SerializeField] float timerfastrec = 0;
    void Update_CheckFastRecollection()
    {
        if (calculate_fast_recollection)
        {
            if (timerfastrec < time_to_fast_recollection)
            {
                timerfastrec = timerfastrec + 1 * Time.deltaTime;
            }
            else
            {
                can_fast_recollecion = true;
            }
        }
        else
        {
            can_fast_recollecion = false;
            timerfastrec = 0;
        }
    }
    [Header("Cooldown")]
    [Range(0f, 0.5f)]
    public float cooldownrecolect = 0.1f;
    public bool cooldown_to_next_recollection = false;
    float timer_cooldown = 0;
    void Update_CooldownNextRecollection()
    {
        if (cooldown_to_next_recollection)
        {
            if (timer_cooldown < cooldownrecolect)
            {
                timer_cooldown = timer_cooldown + 1 * Time.deltaTime;
            }
            else
            {
                cooldown_to_next_recollection = false;
                timer_cooldown = 0;
            }
        }
    }

    bool I_Have_Good_Distace_To_Interact() { return Vector3.Distance(most_close.transform.position, transform.position) < most_close.distancetoInteract; }


}
