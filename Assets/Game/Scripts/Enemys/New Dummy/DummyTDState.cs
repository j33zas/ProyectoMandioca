using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tools.StateMachine
{
    public class DummyTDState : DummyEnemyStates
    {
        float timer;
        float recallTime;

        public DummyTDState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                            float _recall) : base(myState, _sm)
        {
            recallTime = _recall;
        }

        protected override void Enter(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Enter(input);

            //Acá ejecutaría mi animación de Take Damage... ¡¡Si tan solo tuviera una!!x2
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
            timer = 0;
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

            if (timer >= recallTime)
                sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
        }
    }
}
