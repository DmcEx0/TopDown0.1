using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    private Rigidbody _rb;
    private Transform _parent;
    private float _timeAfterSpaw = 0;
    private Vector3 _direction;

    public void Init(Player target, Transform parent)
    {
        _direction = target.transform.position - transform.position;
        _parent = parent;
    }

    public void Init(Enemy target, Transform parent)
    {
        _direction = target.transform.position - transform.position;
        _parent = parent;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _timeAfterSpaw += Time.deltaTime;

        if (_timeAfterSpaw >= _lifeTime)
        {
            TurnOffBullet();    
            _timeAfterSpaw = 0;
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction.normalized * _speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            TurnOffBullet();
            player.ApplyDamage(_damage);
        }
        else if (other.TryGetComponent(out Enemy enemy))
        {
            TurnOffBullet();
            enemy.ApplyDamage(_damage);
        }
    }

    private void TurnOffBullet()
    {
        _timeAfterSpaw = 0;
        gameObject.SetActive(false);
        gameObject.transform.parent = _parent;
    }
}
