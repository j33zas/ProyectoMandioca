using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class DummyAttackState : DummyEnemyStates
    {
        float cd;
        float timer;
        ICombatDirector enemy;


        public DummyAttackState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float _cd, ICombatDirector _enemy) : base(myState, _sm)
        {
            cd = _cd;
            enemy = _enemy;
        }

        protected override void Enter(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Enter(input);
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

            timer += Time.deltaTime;

            if (timer >= cd)
            {
                sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
            }
        }
    }
}
