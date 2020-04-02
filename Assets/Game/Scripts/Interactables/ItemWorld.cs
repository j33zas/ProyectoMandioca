using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//implementar items
//implementar collector

public class ItemWorld : Interactable
{
    public Item item;

    public FeedbackItem feeback;

    bool onselected;

    Vector3 scalenormal;
    Vector3 scalescaled;
    Vector3 posnormal;
    Vector3 posscaled;
    Transform model;

    public UnityEvent to_collect;

    public UnityEvent OnCreate;


    private void Awake()
    {
        feeback = GetComponentInChildren<FeedbackItem>();
        var aux = GetComponentInChildren<ParentFinder>();
        if (aux)
        {
            model = aux.transform;
            scalenormal = model.localScale;
            scalescaled = scalenormal + scalenormal * 0.5f;
            posnormal = model.transform.position;
            posscaled = new Vector3(posnormal.x, posnormal.y + 1, posnormal.z);
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
        if (model) model.localScale = scalenormal;
        if (feeback) feeback.Hide();
        WorldItemInfo.instance.Hide();
        onselected = false;
    }

    public override void ShowInfo(WalkingEntity entity)
    {
        if (!autoexecute)
        {
            if (model) model.localScale = scalescaled;
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
            if (feeback) feeback.Show();
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
