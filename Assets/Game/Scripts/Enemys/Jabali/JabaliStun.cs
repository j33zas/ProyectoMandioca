using System;

namespace Tools.StateMachine
{
    public class JabaliStun : JabaliStates
    {
        Action<JabaliEnemy.JabaliInputs> EnterStun;
        Action<string> UpdateStun;
        Action<JabaliEnemy.JabaliInputs> ExitStun;

        public JabaliStun(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, Action<JabaliEnemy.JabaliInputs> _Enter,
                          Action<string> _Update, Action<JabaliEnemy.JabaliInputs> _Exit) : base(myState, _sm)
        {
            EnterStun = _Enter;
            UpdateStun = _Update;
            ExitStun = _Exit;
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            EnterStun(input);

        }

        protected override void Update()
        {
            UpdateStun(lastState.Name);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            ExitStun(input);
        }
    }
}

