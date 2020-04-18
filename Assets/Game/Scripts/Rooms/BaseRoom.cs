using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : MonoBehaviour
{
    public SensorForEnemysInRoom _sensor;
    public List<EnemyBase> myEnemies = new List<EnemyBase>();


    void Start()
    {
        _sensor = GetComponentInChildren<SensorForEnemysInRoom>();
        _sensor.SubscribeAction(EnterTheRoom);
        _sensor.SubscribeExitAction(ExitRoom);
        Main.instance.SetRoom(this);
        myEnemies = new List<EnemyBase>();
        var enemysInChilderen = GetComponentsInChildren<EnemyBase>();
        foreach (var item in enemysInChilderen)
        {
            var enemys = item.GetComponent<EnemyBase>();
            if (enemys)
            {
                myEnemies.Add(enemys);
            }
        }
        _sensor.myContent.SetActive(false);
    }

    void EnterTheRoom(GameObject player)
    {
        if (player.GetComponent<CharacterHead>())
        {
            _sensor.myContent.SetActive(true);
            Main.instance.SetRoom(this);
            //myEnemies = new List<EnemyBase>();
            //var enemysInChilderen = GetComponentsInChildren<EnemyBase>();
            //foreach(var item in enemysInChilderen)
            //{
            //    var enemys = item.GetComponent<EnemyBase>();
            //    if (enemys)
            //    {
            //        myEnemies.Add(enemys);
            //    }
            //}
        }
    }
    public void ExitRoom(GameObject player)
    {
        if (player.GetComponent<CharacterHead>())
        {
            _sensor.myContent.SetActive(false);
        }
    }

    void CheckEmptyRoom()
    {
        if (myEnemies.Count == 0)
        {
            Debug.Log("empty of enemys");
        }
    }
    public void RemoveEnemyFromList(EnemyBase enemy)
    {
        myEnemies.Remove(enemy);
    }

    public List<EnemyBase> myenemies()
    {
        return myEnemies;
    }

    public bool VIPInRoom()
    {
        for (int i = 0; i < myEnemies.Count; i++)
        {
            if (myEnemies[i].target)
                return true;

        }
        return false;
    }
}
