using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Tools.StateMachine
{
    public class DummyIdleState : DummyEnemyStates
    {
        Func<bool> IsAttack;
        Func<Transform> MyPos;
        float distanceMin;
        float distanceMax;
        float currentDis;
        ICombatDirector enemy;

        public DummyIdleState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                              Func<bool> _isAttack, Func<Transform> _isTarget, float _disInCom, float _disNormal, ICombatDirector _enemy) : base(myState, _sm)
        {
            IsAttack += _isAttack;
            MyPos += _isTarget;
            distanceMax = _disNormal;
            distanceMin = _disInCom;
            enemy = _enemy;

            myState.OnUpdate += Update;
        }

        protected override void Enter(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Enter(input);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Update()
        {
            base.Update();

            if(enemy.CurrentTarget() != null)
            {
                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                root.forward = new Vector3(myForward.x, 0, myForward.z);

                if (IsAttack())
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.ATTACK);
                else
                {
                    if (enemy.IsInPos())
                    {
                        currentDis = distanceMin;
                    }
                    else
                    {
                        currentDis = distanceMax;
                    }

                    if (Vector3.Distance(enemy.CurrentTarget().transform.position, root.position) >= currentDis)
                    {
                        if (currentDis == distanceMin)
                            combatDirector.GetNewNearPos(enemy);
                        sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.GO_TO_POS);
                    }
                }
            }
        }
    }
}
