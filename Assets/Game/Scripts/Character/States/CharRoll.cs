﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    

    public class CharRoll : CharacterStates
    {
        ParticleSystem evadepart;

        public CharRoll(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, ParticleSystem _evadepart) : base(myState, _sm)
        {
            evadepart = _evadepart;
        }

        protected override void Enter(CharacterHead.PlayerInputs input)
        {
            evadepart.Play();

            charMove.Dash();
        }

        protected override void Update()
        {
            base.Update();
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
            evadepart.Stop();

            base.Exit(input);
        }
    }
}
