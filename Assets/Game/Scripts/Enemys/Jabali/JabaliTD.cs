using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliTD : JabaliStates
    {
        float timeToRecall;
        float timer;

        public JabaliTD(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _recall) : base(myState, _sm)
        {
            timeToRecall = _recall;
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            //setear animacion
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= timeToRecall)
                sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            //setear anim;
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
