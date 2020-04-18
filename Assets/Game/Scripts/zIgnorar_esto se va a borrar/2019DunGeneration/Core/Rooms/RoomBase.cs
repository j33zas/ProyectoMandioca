using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions;
using UnityEngine;

public abstract class RoomBase : MonoBehaviour
{
    [Header("RoomBase")]
    ManagerNodes managernodes;
    Inspectable[] inspectables;
    List<PlayObject> entities;
    protected List<IRoomElementable> elements;
    List<Investigator> investigators;
    

    public void Initialize()
    {
        managernodes = GetComponent<ManagerNodes>();
        inspectables = GetComponentsInChildren<Inspectable>();//
        entities = GetComponentsInChildren<PlayObject>().ToList();
        elements = GetComponentsInChildren<IRoomElementable>().ToList();//
        investigators = GetComponentsInChildren<Investigator>().ToList();//


        if (managernodes != null)
        {
           // managernodes.Initialize();
        }
        foreach (var e in entities) e.Initialize();
        foreach (var e in elements) e.SetmanualRoom(this);
        

        OnInitialize();
    }

    public abstract void OnInitialize();


    /////////////////////////////////////////////////////////////////////////////////////////////
    /// Logica para los Investigadores
    /////////////////////////////////////////////////////////////////////////////////////////////

    public List<Investigator> FindInvestigatorsInRadius(Vector3 position, float radius)
    {
        return position.FindInRadiusByConditionNoPhysics<Investigator>(radius,investigators, CondicionDeRoom);
    }
    public bool CondicionDeRoom(Investigator enm)
    {
        Debug.Log("HAY " + investigators.Count + " Investigators");
        return investigators.Contains(enm);
    }
    public Inspectable[] GetInspectables()
    {
        return inspectables;
    }
}
