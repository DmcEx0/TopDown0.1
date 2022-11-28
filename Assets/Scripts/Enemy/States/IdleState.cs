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

        EnemyBehaviuor.Animator.SetBool("IsVictory", true);
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
    }

    public override void Exit()
    {
        base.Exit();

        EnemyBehaviuor.Animator.SetBool("IsVictory", false);
    }
}
