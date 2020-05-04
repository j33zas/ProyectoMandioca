using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class JabaliPushAttack : JabaliStates
    {
        float pushSpeed;
        Action DealDamage;

        public JabaliPushAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _speed,
                                Action _DealDamage) : base(myState, _sm)
        {
            pushSpeed = _speed;
            DealDamage = _DealDamage;
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            base.Enter(input);

            //setear animacion Push
        }

        protected override void Update()
        {
            rb.velocity = root.forward * pushSpeed;
            DealDamage();

            base.Update();
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            base.Exit(input);
            StopMove();
            //setear animacion deja el push
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

