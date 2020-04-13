using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class DummyFollowState : DummyEnemyStates
    {
        float radiousToAvoidance;
        float avoidWeight;
        Func<float> GetSpeed;
        Func<Transform> GetMyPos;

        TrueDummyEnemy noObs;

        float normalDistance;

        public DummyFollowState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float radAvoid, float voidW, Func<float> speed, Func<Transform> myPos, float distance, TrueDummyEnemy me) : base(myState, _sm)
        {
            radiousToAvoidance = radAvoid;
            avoidWeight = voidW;
            GetSpeed += speed;
            GetMyPos += myPos;
            normalDistance = distance;
            noObs = me;
        }

        protected override void Enter(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Enter(input);

        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            rb.velocity = Vector3.zero;
            //setear animación
        }

        protected override void Update()
        {
            base.Update();

            Vector3 dirForward = (target.position - root.position).normalized;

            root.forward = new Vector3(dirForward.x, 0, dirForward.z);

            if (GetMyPos() == null)
            {
                ObstacleAvoidance(root.forward);
                if (Vector3.Distance(target.position, root.position) <= normalDistance)
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
            }
            else
            {
                Vector3 dir = GetMyPos().position - root.position;
                dir.Normalize();

                Vector3 dirFix = new Vector3(dir.x, 0, dir.z);

                ObstacleAvoidance(dirFix);

                float distanceX = Mathf.Abs(GetMyPos().transform.position.x - root.position.x);
                float distanceZ = Mathf.Abs(GetMyPos().transform.position.z - root.position.z);

                if (distanceX < 0.7f && distanceZ < 0.7f)
                {
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
                }
            }

        }

        Transform obs;
        protected void ObstacleAvoidance(Vector3 dir)
        {
            obs = null;
            var friends = Physics.OverlapSphere(root.position, radiousToAvoidance);
            if (friends.Length > 0)
            {
                foreach (var item in friends)
                {
                    if (item.GetComponent<EntityBase>())
                    {
                        if (item.GetComponent<EntityBase>() != noObs)
                        {
                            if (!obs)
                                obs = item.transform;
                            else if (Vector3.Distance(item.transform.position, root.position) < Vector3.Distance(obs.position, root.position))
                                obs = item.transform;
                        }
                    }

                }
            }

            if (obs)
            {
                Vector3 diraux = (root.position - obs.position).normalized;
                //diraux

                diraux = new Vector3(diraux.x,0, diraux.z);

                dir += diraux * avoidWeight;
            }

            rb.velocity = new Vector3(dir.x * GetSpeed(), rb.velocity.y, dir.z * GetSpeed());

            //setear animación
        }
    }
}
