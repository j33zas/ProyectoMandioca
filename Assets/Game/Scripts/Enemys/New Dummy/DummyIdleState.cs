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
        float rotationSpeed;
        float currentDis;
        ICombatDirector enemy;

        public DummyIdleState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                              Func<bool> _isAttack, Func<Transform> _isTarget, float _disInCom, float _disNormal, float _rotSpeed, ICombatDirector _enemy) : base(myState, _sm)
        {
            IsAttack += _isAttack;
            MyPos += _isTarget;
            distanceMax = _disNormal;
            distanceMin = _disInCom;
            enemy = _enemy;
            rotationSpeed = _rotSpeed;
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
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                root.forward = Vector3.Lerp(root.forward, forwardRotation, rotationSpeed * Time.deltaTime);

                if (IsAttack())
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.BEGIN_ATTACK);
                else
                {
                    if (enemy.IsInPos())
                    {
                        Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                        Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);
                        Vector3 pos3 = new Vector3(enemy.CurrentTargetPos().position.x, 0, enemy.CurrentTargetPos().position.z);

                        if (Vector3.Distance(pos1, pos2) >= distanceMin && Vector3.Distance(pos1, pos3) >= 1)
                        {
                            combatDirector.GetNewNearPos(enemy);
                            sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.GO_TO_POS);
                        }
                    }
                    else
                    {
                        Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                        Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);

                        if (Vector3.Distance(pos1, pos2) >= distanceMax)
                        {
                            sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.GO_TO_POS);
                        }
                    }
                }
            }
        }
    }
}
