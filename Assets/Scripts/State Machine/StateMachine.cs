using Unity.VisualScripting;
using UnityEngine;

public class StateMachine
{
    public State state;

    public State GetState => state;

    public void SetState(State newState, bool forceSet = false)
    {
        if (state != newState || forceSet)
        {
            // Debug.Log("Changing form " + state + " to " + newState);
            if (state != null) state.Exit();
            state = newState;
            state.Initialize();
            state.Enter();
        }   
    }
}
