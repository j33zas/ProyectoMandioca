using System.Collections.Generic;
using System;

namespace Tools.StateMachine
{
	public class EventStateMachine<T>
	{
		private EState<T> current;
		Action<string> debug = delegate { };
		public EventStateMachine(EState<T> initial, Action<string> _debug)
		{
			debug = _debug;
			current = initial;
			current.Enter(default(T));
		}
		public void SendInput(T input)
		{
			EState<T> newState;
			if (current.CheckInput(input, out newState))
			{
				current.Exit(input);
				current = newState;
				debug(current.Name);
				current.Enter(input);
			}
		}

		public EState<T> Current { get { return current; } }
		public void Update() { current.Update(); }
		public void LateUpdate() { current.LateUpdate(); }
		public void FixedUpdate() { current.FixedUpdate(); }

	}
}
