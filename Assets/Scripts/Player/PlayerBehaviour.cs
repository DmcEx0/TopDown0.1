using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : ObjectPool, IMovable
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Bullet _bulletTemplate;
    [SerializeField] private float _delay;

    private PlayerController _playerController;
    private Rigidbody _rb;
    private float _timeAfterShot;

    public void Move()
    {
        float minimalValue = 0.1f;

        if (_playerController.Direction.sqrMagnitude < minimalValue)
            return;

        float scaleMoveSpeed = _speed * Time.fixedDeltaTime;

        Vector2 direction = _playerController.Direction;

        Vector3 moveDirection = new Vector3(direction.x, 0,direction.y);

        _rb.MovePosition(_rb.position + moveDirection * scaleMoveSpeed);
    }

    public void Shoot(Enemy target)
    {
        _timeAfterShot += Time.deltaTime;

        if (_timeAfterShot >= _delay)
        {
            if (TryGetObject(out GameObject bullet))
                SetBullet(bullet, target);

            _timeAfterShot = 0;
        }
    }

    private void Start()
    {
        Initialize(_bulletTemplate.gameObject);
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void SetBullet(GameObject bulletTemplate, Enemy target)
    {
        bulletTemplate.SetActive(true);
        bulletTemplate.transform.position = _shootPoint.transform.position;
        bulletTemplate.transform.parent = null;

        Bullet bullet = bulletTemplate.GetComponent<Bullet>();
        bullet.Init(target.gameObject, Container.transform);
    }
}