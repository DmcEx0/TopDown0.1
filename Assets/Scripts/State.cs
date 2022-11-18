using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected EnemyBehaviour EnemyBehaviuor;
    protected StateMachine StateMachine;

    protected State(EnemyBehaviour enemyBehaviour, StateMachine stateMachine)
    {
        this.EnemyBehaviuor = enemyBehaviour;
        this.StateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
