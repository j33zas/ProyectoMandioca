﻿namespace Tools.StateMachine
{
    public class JabaliDeath : JabaliStates
    {
        public JabaliDeath(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            anim.SetBool("Dead", true);
        }
    }
}