﻿namespace Tools.StateMachine
{
    public class DummyDieState : DummyEnemyStates
    {
        public DummyDieState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm) : base(myState, _sm) { }
        protected override void Enter(TrueDummyEnemy.DummyEnemyInputs input) => anim.SetBool("dead", true);
    }
}
