using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//implementar items
//implementar collector

public class ItemWorld : Interactable
{
    [Header("Item World Setup")]
    public Item item;

    bool onselected;

    bool canAnimate;
    [SerializeField] bool animationToChar;

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
    private void Update()
    {
        if (onselected)
        {
            if (model) model.Rotate(0, 20 * Time.deltaTime, 0);
        }
    }
    public void OnAppearInScene()
    {
        OnCreate.Invoke();
    }

    ///////////////////////////////////////////////////////////////////
    ///// PROPIAS DE INTERACTABLE (HERENCIA)
    ///////////////////////////////////////////////////////////////////
    public override void Execute(WalkingEntity collector)
    {
        if (animationToChar)
        {
            //canAnimate;
        }
        collector.OnReceiveItem(this);
        to_collect.Invoke();
        Destroy(this.gameObject);
    }

    public override void Exit()
    {
        if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Hide();

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

            if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Show();


            onselected = true;

        }
        else
        {
            //para el auto execute
            Execute(entity);
        }

    }

}
