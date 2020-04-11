using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    List<ICombatDirector> toAttack = new List<ICombatDirector>();
    List<ICombatDirector> inAttack = new List<ICombatDirector>();
    List<ICombatDirector> waitToAttack = new List<ICombatDirector>();
    List<Transform> positionsToAttack = new List<Transform>();
    [SerializeField, Range(1, 8)] int maxEnemies = 1;
    Transform head;

    bool run;
    float timer;
    float timeToAttack;
    [SerializeField] float timerMin=1;
    [SerializeField] float timerMax=5;

    private void Start()
    {
        head = Main.instance.GetChar().transform;

        Vector3 east = head.position + Vector3.right * 2;
        Vector3 north = head.position + Vector3.forward * 2;
        Vector3 northEast = head.position + (Vector3.right + Vector3.forward) * 1.9f;
        Vector3 northWest = head.position + (Vector3.forward + Vector3.left) * 1.9f;

        positionsToAttack.Add(CreateNewPos(east));
        positionsToAttack.Add(CreateNewPos(-east));
        positionsToAttack.Add(CreateNewPos(north));
        positionsToAttack.Add(CreateNewPos(-north));
        positionsToAttack.Add(CreateNewPos(northEast));
        positionsToAttack.Add(CreateNewPos(-northEast));
        positionsToAttack.Add(CreateNewPos(northWest));
        positionsToAttack.Add(CreateNewPos(-northWest));

        StartCoroutine(MyCourroutine());
    }

    IEnumerator MyCourroutine()
    {
        yield return new WaitForSeconds(0.5f);
        var enemies = Main.instance.GetEnemies();

        Debug.Log(Main.instance.GetEnemies().Count);

        for (int i = 0; i < enemies.Count; i++)
        {
            AddOrRemoveToList(enemies[i]);
        }
    }

    public void RunDirector()
    {
        run = true; timer = 0;
        CalculateTimer();
    }

    public void StopDirector()
    {
        run = false;
        timer = 0;
    }

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

    Transform CreateNewPos(Vector3 pos)
    {
        var newEmpty = new GameObject("PosToAttack");
        newEmpty.transform.position = pos;
        newEmpty.transform.SetParent(head);
        return newEmpty.transform;
    }

    public void AttackRelease(ICombatDirector e)
    {
        if (inAttack.Contains(e))
        {
            positionsToAttack.Add(e.CurrentTargetPos());
            e.SetTargetPos(null);
            inAttack.Remove(e);
            if (waitToAttack.Count > 0)
                AssignPos();
        }
    }

    public void AddOrRemoveToList(ICombatDirector e)
    {
        if(!toAttack.Contains(e) && !waitToAttack.Contains(e))
        {
            if (toAttack.Count + inAttack.Count < maxEnemies)
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
                toAttack.Remove(e);
                inAttack.Add(e);
            }
            else if (waitToAttack.Contains(e))
            {
                waitToAttack.Remove(e);
            }
        }

        if (toAttack.Count > 0 && !run)
            RunDirector();
        else if (toAttack.Count == 0 && waitToAttack.Count == 0)
            StopDirector();
    }

    private void Update()
    {
        if (run)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                ICombatDirector enemy = toAttack[Random.Range(0, toAttack.Count)];
                enemy.ToAttack();

                CalculateTimer();

                AddOrRemoveToList(enemy);
            }
        }
    }

    void CalculateTimer() => timeToAttack = Random.Range(timerMin, timerMax);
}
