using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : ObjectPool
{
    [SerializeField] private Bullet _bulletTemplate;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _layerMask;

    private StateMachine _stateMachine;
    private Enemy _enemy;
    private Rigidbody _rb;

    public IdleState IdleState { get; private set; }
    public FollowState FollowState { get; private set; }
    public AttackState AttackState { get; private set; }
    public Player Target { get; private set; }

    public LayerMask LayerMask => _layerMask;
    public float AttackRange => _attackRange;

    public void Shoot()
    {
        if (TryGetObject(out GameObject bullet))
            SetBullet(bullet);
    }

    public void Move()
    {
        float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        Vector3 moveDirection  = _enemy.Target.transform.position - transform.position;

        if (Target != null)
        {
            _rb.MovePosition(_rb.position + moveDirection.normalized * scaleMoveSpeed);

            Vector3 direction = _enemy.Target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody>();
        _stateMachine = new StateMachine();
        Initialize(_bulletTemplate.gameObject);

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

    private void SetBullet(GameObject bulletTemplate)
    {
        bulletTemplate.SetActive(true);
        bulletTemplate.transform.position = _shootPoint.transform.position;
        bulletTemplate.transform.parent = null;

        Bullet bullet = bulletTemplate.GetComponent<Bullet>();
        bullet.Init(Target, Container.transform);
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
