﻿using System.Collections;
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

            anim.SetBool("takeDamage", true);
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
            anim.SetBool("takeDamage", false);
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
