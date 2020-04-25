using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class DummyAttackState : DummyEnemyStates
    {
        float cd;
        float timer;
        float rotationSpeed;
        ICombatDirector enemy;

        public DummyAttackState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _cd, float _rotSpeed,ICombatDirector _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
            rotationSpeed = _rotSpeed;
        }

        protected override void Enter(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Enter(input);

            anim.SetBool("Attack", true);
            combatDirector.RemoveToAttack(enemy, enemy.CurrentTarget());
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            if (input != TrueDummyEnemy.DummyEnemyInputs.PETRIFIED)
            {
                timer = 0;
                anim.SetBool("Attack", false);
                var myEnemy = (EnemyBase)enemy;
                myEnemy.attacking = false;
                combatDirector.AddToAttack(enemy, enemy.CurrentTarget());
            }
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

            Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
            Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

            root.forward = Vector3.Lerp(root.forward, forwardRotation, rotationSpeed * Time.deltaTime);

            timer += Time.deltaTime;

            if (timer >= cd)
            {
                sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
            }
        }
    }
}
