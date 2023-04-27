using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public int id = 0;

    public abstract void Init(Entity entity = null);

    public abstract void Entry();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();

}

public abstract class EntityState : State
{
    private Entity _entity;

    public EntityState(Entity entity)
    {
        _entity = entity;
    }
    
}

public abstract class FSMState : State
{
    private FSM _stateMachine;

    public FSMState(FSM stateMachine)
    {
        _stateMachine = stateMachine;
    }
}