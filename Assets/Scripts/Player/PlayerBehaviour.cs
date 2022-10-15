using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : ObjectPool
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Bullet _bulletTemplate;
    [SerializeField] private float _delay;

    private PlayerController _playerController;
    private Rigidbody _rb;
    private float _timeAfterShot;

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


    private void SetBullet(GameObject bulletTemplate, Enemy target)
    {
        bulletTemplate.SetActive(true);
        bulletTemplate.transform.position = _shootPoint.transform.position;
        bulletTemplate.transform.parent = null;

        Bullet bullet = bulletTemplate.GetComponent<Bullet>();
        bullet.Init(target, Container.transform);
    }
}
