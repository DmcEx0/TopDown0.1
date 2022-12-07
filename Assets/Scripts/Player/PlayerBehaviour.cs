using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerBehaviour : MonoBehaviour, IMovable
{
    [Header("Player settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private NavMeshAgent _agent;

    [Header("Bullet settings")]
    [SerializeField] private PlayerBullet _bulletTemplate;
    [SerializeField] private Transform _spawnPoolContainer;
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = false;

    private ObjectPool<PlayerBullet> _bulletPool;

    private Player _player;
    private PlayerController _playerController;
    //private Rigidbody _rb;
    private Animator _animator;
    private float _timeAfterShot;

    public void Move()
    {
        float minimalValue = 0.1f;
        float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        if (_playerController.Direction.sqrMagnitude < minimalValue)
        {
            //_rb.velocity = Vector3.zero;

            return;
        }

        Vector2 direction = _playerController.Direction;

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y)  + transform.position;

        _agent.SetDestination(moveDirection);
    }

    private void Start()
    {
        InitializePool();
        _playerController = GetComponent<PlayerController>();
        //_rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();

        _agent.speed = _speed;
        _agent.angularSpeed = _angularSpeed;
    }

    private void Update()
    {
        Move();

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
        playerBullet.SetDamage(_player.Damage);
    }

    private void TurnToTarget()
    {
        Vector3 directionTarget = _player.Target.transform.position - transform.position;
        Vector3 lookDirection = new Vector3(directionTarget.x, 0, directionTarget.z);
        Quaternion rotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _angularSpeed * Time.deltaTime);
    }

    private void Shoot(Enemy target)
    {
        _timeAfterShot += Time.deltaTime;

        if (_timeAfterShot >= _player.DelayBeforeFiring)
        {
            _animator.SetTrigger("IsShooting");
            SetBullet(target);
            _timeAfterShot = 0;
        }
    }
}