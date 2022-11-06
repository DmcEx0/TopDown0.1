using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBehaviour : MonoBehaviour, IMovable
{
    [SerializeField] private EnemyBullet _bulletTemplate;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = false;
    [SerializeField] private Transform _spawnPoolContainer;

    private PoolMono<EnemyBullet> _bulletPool;

    private StateMachine _stateMachine;
    private Enemy _enemy;
    private Rigidbody _rb;
    private Collider _collider;

    public IdleState IdleState { get; private set; }
    public FollowState FollowState { get; private set; }
    public AttackState AttackState { get; private set; }
    public Player Target { get; private set; }

    public LayerMask LayerMask => _layerMask;
    public float AttackRange => _attackRange;
    public float RotationSpeed => _rotationSpeed;

    public void Shoot()
    {
        SetBullet();
    }

    public void Move()
    {
        if (Target == null)
            return;

        float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        Vector3 direction = _enemy.Target.transform.position - transform.position;

        _rb.MovePosition(_rb.position + direction.normalized * scaleMoveSpeed);

        Vector3 lookDirection = _enemy.Target.transform.position - _rb.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody>();
        _stateMachine = new StateMachine();

        InitializePool();

        IdleState = new IdleState(this, _stateMachine);
        FollowState = new FollowState(this, _stateMachine);
        AttackState = new AttackState(this, _stateMachine);

        _stateMachine.Initialize(IdleState);

        Target = _enemy.Target;
    }

    private void Update()
    {
        _stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();
    }

    private void InitializePool()
    {
        _bulletPool = new PoolMono<EnemyBullet>(_bulletTemplate, _poolCount, _spawnPoolContainer.transform);
        _bulletPool.IsAutoExpand = _isAutoExpand;
    }

    private void SetBullet()
    {
        EnemyBullet bulletEnemy = _bulletPool.GetFreeElement();
        bulletEnemy.transform.position = _shootPoint.position;
        bulletEnemy.transform.parent = null;

        bulletEnemy.Init(Target.gameObject, _spawnPoolContainer.transform);
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
