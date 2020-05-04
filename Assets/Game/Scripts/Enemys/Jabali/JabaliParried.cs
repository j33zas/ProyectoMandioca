using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliParried : JabaliStates
    {
        float timeParry;
        float timer;

        public JabaliParried(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _timeParried) : base(myState, _sm)
        {
            timeParry = _timeParried;
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            //setear animacion de que fue parreado

        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= timeParry)
                sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                timer = 0;
                //setear animacion
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

