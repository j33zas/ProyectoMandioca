using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tools.StateMachine
{
    public class DummyDieState : DummyEnemyStates
    {
        public DummyDieState(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm) : base(myState, _sm)
        {

        }

        protected override void Enter(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Enter(input);

            anim.SetBool("dead", true);
            //rb.velocity = Vector3.zero;
            //Debug.Log(rb.velocity);
            //rb.useGravity = false;
            //rb.GetComponent<BoxCollider>().isTrigger = true;
            
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
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

        protected override void Update()
        {
            base.Update();
        }
    }
}
