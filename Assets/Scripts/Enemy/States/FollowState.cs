using UnityEngine;

public class FollowState : State
{
    private Collider[] _targetBuffer = new Collider[1];
    private float _attackRange;
    private float _permissibleError = 0.3f;

    public FollowState(EnemyBehaviour enemyBehaviour, StateMachine stateMachine) : base(enemyBehaviour, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _attackRange = EnemyBehaviuor.AttackRange - _permissibleError;
        EnemyBehaviuor.Animator.SetBool("IsRunning", true);
        EnemyBehaviuor.Agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (EnemyBehaviuor.Target == null)
            StateMachine.ChangeState(EnemyBehaviuor.IdleState);

        int hits = Physics.OverlapSphereNonAlloc(EnemyBehaviuor.transform.position, _attackRange, _targetBuffer, EnemyBehaviuor.LayerMask);

        if (hits > 0)
            StateMachine.ChangeState(EnemyBehaviuor.AttackState);

        EnemyBehaviuor.Move();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        EnemyBehaviuor.Animator.SetBool("IsRunning", false);
        EnemyBehaviuor.Agent.isStopped = true;
    }
}
