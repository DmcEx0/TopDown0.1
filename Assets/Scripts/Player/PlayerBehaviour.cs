using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : ObjectPool
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Bullet _bulletTemplate;

    private PlayerController _playerController;
    private Rigidbody _rb;
    private Coroutine _doShootJob;

    public void StartDoShootCoroutine(Enemy target)
    {
        _doShootJob = StartCoroutine(DoShoot(target));
    }

    public void StopDoShootCoroutine()
    {
        if (_doShootJob != null)
            StopCoroutine(_doShootJob);
    }

    private void Start()
    {
        Initialize(_bulletTemplate.gameObject);
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move(_playerController.Direction);
    }

    private void Move(Vector2 direction)
    {
        float minimalValue = 0.1f;

        if (direction.sqrMagnitude < minimalValue)
            return;

        float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);

        _rb.MovePosition(_rb.position + moveDirection * scaleMoveSpeed);
    }

    private IEnumerator DoShoot(Enemy target)
    {
        WaitForSeconds delay = new WaitForSeconds(1);

        while (target != null)
        {
            Shoot(target);

            yield return delay;
        }
    }

    private void Shoot(Enemy target)
    {
        if (TryGetObject(out GameObject bullet))
            SetBullet(bullet, target);
    }

    private void SetBullet(GameObject bulletTemplate, Enemy target)
    {
        bulletTemplate.SetActive(true);
        bulletTemplate.transform.position = _shootPoint.transform.position;
        bulletTemplate.transform.parent = null;

        Bullet bullet = bulletTemplate.GetComponent<Bullet>();
        bullet.Init(target, Container.transform);
    }
}
