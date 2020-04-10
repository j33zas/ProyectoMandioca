using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class DummyEnemyStates<T> : StatesFunctions<T>
    {
        protected EState<T> lastState;

        public DummyEnemyStates(EState<T> myState, EventStateMachine<T> _sm) : base(myState, _sm)
        {

        }

        protected override void Enter(T input)
        {

        }

        protected override void Exit(T input)
        {
            lastState = sm.Current;
        }

        protected override void FixedUpdate()
        {

        }

        protected override void LateUpdate()
        {

        }

        protected override void Update()
        {

        }
    }
}
