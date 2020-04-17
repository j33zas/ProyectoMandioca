using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonGenerator.Components;

public class ChangeNode : MonoBehaviour, IDungeonElement
{

   // public NodeAgua nodeagua;

    public void OnDungeonGenerationFinallized()
    {
        
    }

    public void OnPlayerEnterInThisRoom(Transform who)
    {
        //var handler = NewRoomGenerator.instancia.currentroom.GetComponentInChildren<PlaceChangerHandler>();
        //if (handler) handler.AddNode(this);
    }

    public void OnInstatiate()
    {
        //var handler = NewRoomGenerator.instancia.currentroom.GetComponentInChildren<PlaceChangerHandler>();
        //if(handler) handler.AddNode(this);
    }

    public void OnPlayerExitInThisRoom()
    {
        
    }

    public void OnUpdateInThisRoom()
    {
        
    }

    void Start()
    {
        
    }

    public void OnPlayerDeath()
    {
        
    }
}
