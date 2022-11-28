using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private Collider[] _targetBuffer = new Collider[1];

    private Coroutine _shootJob;

    public AttackState(EnemyBehaviour enemyBehaviour, StateMachine stateMachine) : base(enemyBehaviour, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _shootJob = EnemyBehaviuor.StartCoroutine(DoShoot());
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        int hits = Physics.OverlapSphereNonAlloc(EnemyBehaviuor.transform.position, EnemyBehaviuor.AttackRange, _targetBuffer, EnemyBehaviuor.LayerMask);

        if (hits <= 0)
            StateMachine.ChangeState(EnemyBehaviuor.FollowState);

        if (EnemyBehaviuor.Target != null)
        {
            Vector3 direction = EnemyBehaviuor.Target.transform.position - EnemyBehaviuor.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            EnemyBehaviuor.transform.rotation = Quaternion.Lerp(EnemyBehaviuor.transform.rotation, rotation, EnemyBehaviuor.RotationSpeed * Time.fixedDeltaTime);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        EnemyBehaviuor.StopCoroutine(_shootJob);
    }

    private IEnumerator DoShoot()
    {
        WaitForSeconds delay = new WaitForSeconds(1);

        while (EnemyBehaviuor.Target != null)
        {
            EnemyBehaviuor.Animator.SetTrigger("Shoot");
            EnemyBehaviuor.Shoot();
            yield return delay;
        }

        yield break;
    }
}
