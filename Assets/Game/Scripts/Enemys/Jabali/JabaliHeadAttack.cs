using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliHeadAttack : JabaliStates
    {
        float cdToAttack;
        float timer;

        public JabaliHeadAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _cdToAttack) : base(myState, _sm)
        {
            cdToAttack = _cdToAttack;
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED) 
            {
                //setear trigger
            }
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= cdToAttack)
            {
                sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                timer = 0;
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
    }
}
