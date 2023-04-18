using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public Dictionary<System.Type, State> states;
    public State currentState { get; private set; }
    
    public FSM (Dictionary<System.Type, State> states, State initialState)
    {
        this.states = states;
        currentState = initialState;
    }
 
    public void Init(Entity entity)
    {
        if (states == null) states = new Dictionary<System.Type, State>();

        foreach (State state in states.Values)
        {
            state.Init(entity);
        }

        currentState?.Entry();
    }

    public void Tick()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public void Interrupt(bool stateHasExit)
    {
        currentState?.Exit();
    }

}
