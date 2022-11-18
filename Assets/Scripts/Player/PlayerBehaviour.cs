using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour, IMovable
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private PlayerBullet _bulletTemplate;
    [SerializeField] private float _delay;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = false;
    [SerializeField] private Transform _spawnPoolContainer;

    private ObjectPool<PlayerBullet> _bulletPool;

    private Player _player;
    private PlayerController _playerController;
    private Rigidbody _rb;
    private Animator _animator;
    private float _timeAfterShot;
    private bool _isRunning;

    public void Move()
    {
        float minimalValue = 0.1f;

        if (_playerController.Direction.sqrMagnitude < minimalValue)
            return;

        float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        Vector2 direction = _playerController.Direction;

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);

        _rb.MovePosition(_rb.position + moveDirection * scaleMoveSpeed);

        
    }


    private void Start()
    {
        InitializePool();
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_player.IsTargetReceived)
        {
            TurnToTarget();
            Shoot(_player.Target);
        }

        if (_playerController.Direction.x != 0 || _playerController.Direction.y != 0)
            _animator.SetBool("IsRunning", true);
        else
            _animator.SetBool("IsRunning", false);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InitializePool()
    {
        _bulletPool = new ObjectPool<PlayerBullet>(_bulletTemplate, _poolCount, _spawnPoolContainer.transform);
        _bulletPool.IsAutoExpand = _isAutoExpand;
    }

    private void SetBullet(Enemy target)
    {
        PlayerBullet playerBullet = _bulletPool.GetFreeElement();
        playerBullet.transform.position = _shootPoint.position;
        playerBullet.transform.parent = null;

        playerBullet.Init(target.gameObject, _spawnPoolContainer.transform);
    }

    private void TurnToTarget()
    {
        Vector3 direction = _player.Target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }
    private void Shoot(Enemy target)
    {
        _timeAfterShot += Time.deltaTime;

        if (_timeAfterShot >= _delay)
        {
            _animator.SetTrigger("IsShooting");
            SetBullet(target);
            _timeAfterShot = 0;
        }
    }
}