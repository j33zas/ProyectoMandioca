using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//implementar items
//implementar collector

public class ItemWorld : Interactable
{
    public Item item;

    bool onselected;

    Transform model;

    public UnityEvent to_collect;
    public UnityEvent OnCreate;


    private void Awake()
    {
        
        var aux = GetComponentInChildren<ParentFinder>();
        if (aux)
        {
            model = aux.transform;
        }
    }

    public override void Execute(WalkingEntity collector)
    {
        collector.OnReceiveItem(this);
        to_collect.Invoke();
        Destroy(this.gameObject);
    }

    public void OnAppearInScene()
    {
        OnCreate.Invoke();
    }

    public override void Exit()
    {
        if (feedback) feedback.Hide();
        if (feedback2) feedback2.Hide();
        WorldItemInfo.instance.Hide();
        onselected = false;
    }

    public override void ShowInfo(WalkingEntity entity)
    {
        if (!autoexecute)
        {
            if (item)
            {
                if (pointToMessage != null)
                {
                    WorldItemInfo.instance.Show(pointToMessage.position, item.name, item.description);
                }
                else
                {
                    WorldItemInfo.instance.Show(this.transform.position, item.name, item.description);
                }
            }
            if (feedback)
            {
                feedback.Show();
            }
            if (feedback2)
            {
                feedback2.Show();
            }

            onselected = true;

        }
        else
        {
            //para el auto execute
            Execute(entity);
        }

    }

    private void Update()
    {
        if (onselected)
        {
            if (model) model.Rotate(0, 20 * Time.deltaTime, 0);
        }
    }
}
