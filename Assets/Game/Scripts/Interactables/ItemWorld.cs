using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

//implementar items
//implementar collector

public class ItemWorld : Interactable
{
    [Header("Item World Setup")]
    public Item item;

    Item_animRecolect recolector_anim;
    public bool canrecolectoranim;

    public UnityEvent to_collect;
    public UnityEvent OnCreate;

    public ItemInterceptor interceptor;

    private void Awake()
    {
        recolector_anim = GetComponent<Item_animRecolect>();
        if (recolector_anim != null) canrecolectoranim = true;

        interceptor = GetComponent<ItemInterceptor>();
    }
    public void OnAppearInScene()
    {
        OnCreate.Invoke();
    }

    ///////////////////////////////////////////////////////////////////
    ///// PROPIAS DE INTERACTABLE (HERENCIA)
    ///////////////////////////////////////////////////////////////////
    public override void OnExecute(WalkingEntity collector)
    {
        if (canrecolectoranim)
        {
            recolector_anim.BeginRecollect(collector, Collect);
        }
        else
        {
            Collect(collector);
        }

    }

    void Collect(WalkingEntity collector)
    {
        collector.OnReceiveItem(this);
        if (interceptor != null)
        {
            if (interceptor.Collect())
            {
                to_collect.Invoke();
                Destroy(this.gameObject);
            }
        }
        else
        {
            to_collect.Invoke();
            Destroy(this.gameObject);
        }
    }

    public override void OnExit()
    {
        WorldItemInfo.instance.Hide();
    }

    public override void OnEnter(WalkingEntity entity)
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
        }
        else
        {
            //para el auto execute
            Execute(entity);
        }

    }

}
