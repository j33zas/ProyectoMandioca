using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharReleaseAttack : CharacterStates
    {
        float timer;
        float attackRecall;
        public CharReleaseAttack(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall) : base(myState, _sm)
        {
            attackRecall = recall;
        }

        protected override void Enter(CharacterHead.PlayerInputs input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
            charAttack.OnAttackEnd();
        }

        protected override void Update()
        {
            //timer += Time.deltaTime;

            //if (timer >= attackRecall)
            //    sm.SendInput(CharacterHead.PlayerInputs.IDLE);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            timer = 0;
        }
    }
}
