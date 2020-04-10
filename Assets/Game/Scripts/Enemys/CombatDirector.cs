using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Combat director viejo, estaba un poco Acoplado
//el que lo agarre que lo desacople y que lo haga con una
//lista de Enemies

public class CombatDirector : MonoBehaviour
{
    List<ICombatDirector> toAttack = new List<ICombatDirector>();
    List<ICombatDirector> waitToAttack = new List<ICombatDirector>();
    List<Transform> positionsToAttack = new List<Transform>();
    [SerializeField] int maxEnemies;

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

    void AssignPos()
    {
        ICombatDirector randomEnemy = waitToAttack[Random.Range(0, waitToAttack.Count)];

        waitToAttack.Remove(randomEnemy);
        toAttack.Add(randomEnemy);

        AssignPos(randomEnemy);
    }

    void AssignPos(ICombatDirector e)
    {
        Transform toFollow = GetNearPos(e.CurrentPos());

        e.SetTargetPos(toFollow);
    }

    Transform GetNearPos(Vector3 p)
    {
        Transform current = null;

        for (int i = 0; i < positionsToAttack.Count; i++)
        {
            if (current == null)
            {
                current = positionsToAttack[i];
            }
            else
            {
                if (Vector3.Distance(current.position, p) > Vector3.Distance(positionsToAttack[i].position, p))
                    current = positionsToAttack[i];
            }
        }

        positionsToAttack.Remove(current);

        return current;
    }

    void CreateNewPos(Vector3 pos)
    {

    }

    public void AddOrRemoveToList(ICombatDirector e)
    {
        if(!toAttack.Contains(e) && !waitToAttack.Contains(e))
        {
            if (toAttack.Count < maxEnemies)
            {
                toAttack.Add(e);
                AssignPos(e);
            }
            else
            {
                waitToAttack.Add(e);
            }

        }
        else
        {
            if (toAttack.Contains(e))
            {
                positionsToAttack.Add(e.CurrentTargetPos());
                e.SetTargetPos(null);
                toAttack.Remove(e);
                if (waitToAttack.Count > 0)
                    AssignPos();
            }
            else if (waitToAttack.Contains(e))
            {
                waitToAttack.Remove(e);
            }
        }
    }

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
