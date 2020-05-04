using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliCharge : JabaliStates
    {
        float chargeTime;
        float timer = 0;

        public JabaliCharge(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _chargeTime) : base(myState, _sm)
        {
            chargeTime = _chargeTime;
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                //setear booleano de la animación
                combatDirector.RemoveToAttack(enemy, enemy.CurrentTarget());
            }
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= chargeTime)
            {
                sm.SendInput(JabaliEnemy.JabaliInputs.PUSH);
            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                //setear animación para que no cause problemas
                timer = 0;
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
