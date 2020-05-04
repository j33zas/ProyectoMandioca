using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliDeath : JabaliStates
    {
        public JabaliDeath(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            //setear animacion
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
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
    }
}
