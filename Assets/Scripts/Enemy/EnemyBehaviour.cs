using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBehaviour : MonoBehaviour, IMovable
{
    [SerializeField] private float _speed = 4;
    [SerializeField] private float _angularSpeed = 100;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private NavMeshAgent _agent;

    //private Rigidbody _rb;
    private Animator _animator;
    private StateMachine _stateMachine;

    protected Enemy Enemy;

    public IdleState IdleState { get; private set; }
    public FollowState FollowState { get; private set; }
    public AttackState AttackState { get; private set; }

    public Player Target { get; private set; }
    public NavMeshAgent Agent => _agent;
    //public Rigidbody Rigidbody => _rb;

    public LayerMask LayerMask => _layerMask;
    public float AttackRange => _attackRange;
    public float AngularSpeed => _angularSpeed;
    public Animator Animator => _animator;
    public float DelayBeforeFiring => Enemy.DelayBeforeFiring;

    public abstract void Shoot();

    public void Move()
    {
        if (Target == null)
            return;

        _agent.SetDestination(Target.transform.position);

        //float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        //Vector3 direction = Enemy.Target.transform.position - transform.position;

        //_rb.MovePosition(_rb.position + direction.normalized * scaleMoveSpeed);


        //Vector3 targetDirection = Enemy.Target.transform.position - _rb.position;
        //Vector3 lookDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
        //Quaternion rotation = Quaternion.LookRotation(lookDirection);

        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _angularSpeed * Time.fixedDeltaTime);
    }

    private void Start()
    {
        _stateMachine = new StateMachine();
        _animator = GetComponent<Animator>();
        Enemy = GetComponent<Enemy>();
        //_rb = GetComponent<Rigidbody>();

        IdleState = new IdleState(this, _stateMachine);
        FollowState = new FollowState(this, _stateMachine);
        AttackState = new AttackState(this, _stateMachine);

        Target = Enemy.Target;
        _stateMachine.Initialize(IdleState);

        _agent.speed = _speed;
        _agent.angularSpeed = _angularSpeed;
    }

    private void Update()
    {
        _stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        Gizmos.DrawWireSphere(position, _attackRange);

        if (Target != null)
            Gizmos.DrawLine(position, Target.transform.position);
    }
}
