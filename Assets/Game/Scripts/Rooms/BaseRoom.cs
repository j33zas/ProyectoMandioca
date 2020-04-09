using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : MonoBehaviour
{
    SensorForEnemysInRoom _sensor;
    List<EnemyBase> myEnemies = new List<EnemyBase>();
    // Start is called before the first frame update
    void Start()
    {
        _sensor = FindObjectOfType<SensorForEnemysInRoom>();
        _sensor.SubscribeAction(EnterTheRoom);
        _sensor.SubscribeExitAction(ExitRoom);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnterTheRoom(GameObject player)
    {
        if (player.GetComponent<CharacterHead>())
        {
            _sensor.myContent.SetActive(true);
            Main.instance.SetRoom(this);
            myEnemies = new List<EnemyBase>();

           myEnemies= _sensor.MyEnemys;
           
            int index = Random.Range(0, myEnemies.Count);
            myEnemies[index].IsTarget();
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].On();
                if (i != index)
                    myEnemies[i].IsNormal();
            }
        }
    }
    void ExitRoom(GameObject player)
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
}
