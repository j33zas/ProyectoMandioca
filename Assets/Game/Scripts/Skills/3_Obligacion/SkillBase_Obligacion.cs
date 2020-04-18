using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase_Obligacion : SkillBase
{
    protected EnemyBase enemyVIP;
    protected List<EnemyBase> myEnemies = new List<EnemyBase>();
    protected SensorForEnemysInRoom mySensor;

    protected override void OnBeginSkill()
    {
        mySensor = Main.instance.GetRoom()._sensor;
        mySensor.AddCallback_OnTriggerExit(ExitTheRoom);
        myEnemies = Main.instance.GetRoom().myenemies();
        Debug.Log(Main.instance.GetRoom());
        if (!Main.instance.GetRoom().VIPInRoom())
        {
            int index = Random.Range(0, myEnemies.Count);
            myEnemies[index].IsTarget();
            enemyVIP= myEnemies[index];
            Debug.Log(myEnemies[index]);
            for (int i = 0; i < myEnemies.Count; i++)
            {
                Debug.Log(myEnemies[i]);
                myEnemies[i].Resume();
                myEnemies[i].IAInitialize(Main.instance.GetCombatDirector());
                if (i != index)
                    myEnemies[i].IsNormal();
            }
        }
       
    }

    protected override void OnEndSkill()
    {
        //mySensor.RemoveEventListener_OnTriggerExit(ExitTheRoom);
        //Main.instance.GetRoom().ExitRoom();
    }

    protected override void OnUpdateSkill()
    {
        
    }
    protected void ExitTheRoom(GameObject player)
    {
        if (player.GetComponent<CharacterHead>())
        {
            mySensor.myContent.SetActive(false);
            //aca podrias poner el onEndSkill()
        }

    }
}
