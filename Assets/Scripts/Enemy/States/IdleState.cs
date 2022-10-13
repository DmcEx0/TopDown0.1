using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(EnemyBehaviour enemyBehaviour, StateMachine stateMachine) : base(enemyBehaviour, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (EnemyBehaviuor.Target != null)
            StateMachine.ChangeState(EnemyBehaviuor.FollowState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Rotate();
    }

    private void Rotate() // ????????????????
    {
        float a = 0;
        a += 10;
        Vector3 rotate = new Vector3(0f, a, 0f);
        EnemyBehaviuor.transform.Rotate(rotate);
    }
}
