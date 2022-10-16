using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBehaviour : ObjectPool, IMovable
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
    public float RotationSpeed => _rotationSpeed;

    public void Shoot()
    {
        if (TryGetObject(out GameObject bullet))
            SetBullet(bullet);
    }

    public void Move()
    {
        if (_enemy.Target == null)
            return;

        float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        Vector3 direction = _enemy.Target.transform.position - transform.position;

        _rb.MovePosition(_rb.position + direction.normalized * scaleMoveSpeed);

        if (Target != null)
        {
            Vector3 lookDirection = _enemy.Target.transform.position - _rb.position;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
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
        bullet.Init(Target.gameObject, Container.transform);
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
