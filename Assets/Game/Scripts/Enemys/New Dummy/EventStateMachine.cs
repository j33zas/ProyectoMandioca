using System.Collections.Generic;

namespace Tools.StateMachine
{
	public class EventStateMachine<T>
	{
		private EState<T> current;
		public EventStateMachine(EState<T> initial)
		{
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
				current.Enter(input);
			}
		}

		public EState<T> Current { get { return current; } }
		public void Update() { current.Update(); }
		public void LateUpdate() { current.LateUpdate(); }
		public void FixedUpdate() { current.FixedUpdate(); }

	}
}
