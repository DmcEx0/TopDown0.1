using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBehaviour : MonoBehaviour, IMovable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _layerMask;

    private StateMachine _stateMachine;
    private Rigidbody _rb;
    private Animator _animator;

    private OgreBehaviour _ogreBehaviour;
    private MageBehaviour _mageBehaviour;

    protected Enemy Enemy;

    public IdleState IdleState { get; private set; }
    public FollowState FollowState { get; private set; }
    public AttackState AttackState { get; private set; }
    public Player Target { get; private set; }

    public LayerMask LayerMask => _layerMask;
    public float AttackRange => _attackRange;
    public float RotationSpeed => _rotationSpeed;
    public Animator Animator => _animator;

    public void Move()
    {
        if (Target == null)
            return;

        float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        Vector3 direction = Enemy.Target.transform.position - transform.position;

        _rb.MovePosition(_rb.position + direction.normalized * scaleMoveSpeed);

        Vector3 targetDirection = Enemy.Target.transform.position - _rb.position;
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    public void Shoot()
    {
        if (_ogreBehaviour != null)
            _ogreBehaviour.OgreAttacked();

        if (_mageBehaviour != null)
            _mageBehaviour.MageAttacked();
    }

    private void Start()
    {
        _ogreBehaviour = GetComponent<OgreBehaviour>();
        _mageBehaviour = GetComponent<MageBehaviour>();

        _animator = GetComponent<Animator>();
        Enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody>();
        _stateMachine = new StateMachine();

        IdleState = new IdleState(this, _stateMachine);
        FollowState = new FollowState(this, _stateMachine);
        AttackState = new AttackState(this, _stateMachine);

        _stateMachine.Initialize(IdleState);

        Target = Enemy.Target;
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
