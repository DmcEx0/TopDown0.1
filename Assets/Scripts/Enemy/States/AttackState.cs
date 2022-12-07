using System.Collections;
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
        //EnemyBehaviuor.Rigidbody.velocity = Vector3.zero;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        int hits = Physics.OverlapSphereNonAlloc(EnemyBehaviuor.transform.position, EnemyBehaviuor.AttackRange, _targetBuffer, EnemyBehaviuor.LayerMask);

        if (hits <= 0)
            StateMachine.ChangeState(EnemyBehaviuor.FollowState);

        if (EnemyBehaviuor.Target != null)
        {
            Vector3 targetDirection = EnemyBehaviuor.Target.transform.position - EnemyBehaviuor.transform.position;
            Vector3 lookDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
            Quaternion rotation = Quaternion.LookRotation(lookDirection);

            EnemyBehaviuor.transform.rotation = Quaternion.Lerp(EnemyBehaviuor.transform.rotation, rotation, EnemyBehaviuor.AngularSpeed * Time.fixedDeltaTime);
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
        WaitForSeconds delay = new WaitForSeconds(EnemyBehaviuor.DelayBeforeFiring);

        while (EnemyBehaviuor.Target != null)
        {
            EnemyBehaviuor.Animator.SetTrigger("Shoot");
            EnemyBehaviuor.Shoot();
            yield return delay;
        }

        yield break;
    }
}
