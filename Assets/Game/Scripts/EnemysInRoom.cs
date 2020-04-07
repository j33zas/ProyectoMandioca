using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysInRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var enemysInRoom = FindObjectsOfType<EnemyBase>();
        int index = Random.Range(0, enemysInRoom.Length-1);
        enemysInRoom[index].IsTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
