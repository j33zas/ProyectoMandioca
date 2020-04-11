using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class DummyStunState : DummyEnemyStates
    {
        float currentAnimVel;

        float timer;
        float timePetrify;

        EState<TrueDummyEnemy.DummyEnemyInputs> attackState;

        public DummyStunState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm, float _petrify,
                              EState<TrueDummyEnemy.DummyEnemyInputs> _attackState) : base(myState, _sm)
        {
            timePetrify = _petrify;
            attackState = _attackState;
        }

        protected override void Enter(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Enter(input);
            currentAnimVel = anim.speed;
            anim.speed = 0;
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);
            anim.speed = currentAnimVel;
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

            if (timer >= timePetrify)
            {
                if (lastState == attackState)
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.ATTACK);
                else
                    sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
            }
        }
    }
}
