﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharBeginBlock : CharacterStates
    {
        public CharBeginBlock(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(CharacterHead.PlayerInputs input)
        {
            charBlock.OnBlock();
        }

        protected override void Update()
        {
            charMove.RotateHorizontal(RightHorizontal());
            charMove.RotateVertical(RightVertical());
            charMove.MovementHorizontal(LeftHorizontal());
            charMove.MovementVertical(LeftVertical());
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
            if(input != CharacterHead.PlayerInputs.BLOCK)
            {
                charBlock.UpBlock();
            }
        }
    }
}
