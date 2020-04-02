using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Combat director viejo, estaba un poco Acoplado
//el que lo agarre que lo desacople y que lo haga con una
//lista de Enemies

public class CombatDirector : MonoBehaviour
{
    //public List<State_InPosToAttack> subs = new List<State_InPosToAttack>();
    bool run;
    float timer;
    float time_to_next_attack;
    public float time_MIN_Attack;
    public float time_MAX_Attack;

    //RoomTriggerManager manager;
    private void Awake()
    {
        //manager = GetComponent<RoomTriggerManager>();
    }

    //public void Subscribe_ToDirection(State_InPosToAttack _sub) {
    //   // if (!subs.Contains(_sub)) subs.Add(_sub);
    //   // if (subs.Count > 0) RunDirector();
    //}
    //public void UnSubscribe_ToDirection(State_InPosToAttack _sub) {
    //   // if (subs.Contains(_sub)) subs.Remove(_sub);
    //   // if (subs.Count < 1) StopDirector();
    //}
    public void RunDirector() {
        //manager.Alert();
        run = true; timer = 0; Calculate_Time_To_Next_Attack(); }
    public void StopDirector() { run = false; timer = 0; }


    private void Update()
    {
        if (run)
        {
            if (timer < time_to_next_attack)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                //subs[Random.Range(0, subs.Count)].Attack();
                Calculate_Time_To_Next_Attack();
            }
        }
    }

    void Calculate_Time_To_Next_Attack() => time_to_next_attack = Random.Range(time_MIN_Attack, time_MAX_Attack);
}
