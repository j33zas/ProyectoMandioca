namespace Tools.StateMachine
{
    using input = TrueDummyEnemy.DummyEnemyInputs;
    public class DummyStunState : DummyEnemyStates
    {
        EState<input> attackState;
        float currentAnimVel, timePetrify, timer;
        bool startTimer;
        public DummyStunState(EState<input> myState, EventStateMachine<input> _sm, float _petrify,
                              EState<input> _attackState) : base(myState, _sm)
        {
            timePetrify = _petrify;
            attackState = _attackState;
        }
        protected override void Enter(input input)
        {
            base.Enter(input);
            currentAnimVel = anim.speed;
            anim.speed = 0;
            startTimer = true;
        }
        protected override void Update()
        {
            if (startTimer)
            {
                timer *= UnityEngine.Time.deltaTime;
                if (timer >= timePetrify)
                    sm.SendInput(lastState.Name == attackState.Name ?
                        input.ATTACK : (lastState.Name == "Begin_Attack" ?
                        input.BEGIN_ATTACK : input.IDLE));
            }
        }
        protected override void Exit(input input)
        {
            base.Exit(input);
            startTimer = false;
            anim.speed = currentAnimVel;
        }
    }
}
