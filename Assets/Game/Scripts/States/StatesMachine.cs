using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesMachine
{
    States Current;
    List<States> StateList = new List<States>();

    public void Update()
    {
        if (Current != null)
        {
            Current.Execute();
        }
    }
    public void Addstate(States state)
    {
        StateList.Add(state);
        if (Current == null)
        {
            Current = state;
        }
    }
    public void ChangeState<T>() where T : States
    {
        for (int i = 0; i < StateList.Count; i++)
        {
            if (StateList[i].GetType() == typeof(T))
            {
                Current.Sleep();
                Current = StateList[i];
                Current.Start();
            }
        }
    }
    public bool IsActualState<T>() where T : States
    {
        return Current.GetType() == typeof(T);
    }
    
    public T GetState<T>() where T : States
    {
        foreach (States st in StateList)
        {
            if (st.GetType() == typeof(T))
                return st as T;
        }

        return null;
    }
}
