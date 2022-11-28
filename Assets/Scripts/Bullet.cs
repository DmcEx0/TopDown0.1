using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Bullet : MonoBehaviour, IMovable
{
    [SerializeField] private float _lifeTime;
    [SerializeField] protected float Speed;

    private Transform _parent;
    private float _timeAfterSpaw;

    private Rigidbody _rb;
    private Vector3 _direction;

    protected int Damage;

    public void Init(GameObject target, Transform parent)
    {
        _direction = target.transform.position - transform.position;
        _parent = parent;
    }

    public void Move()
    {
        _rb.MovePosition(_rb.position + _direction.normalized * Speed * Time.fixedDeltaTime);
    }

    public void SetDamage(int damage)
    {
        Damage = damage;
    }

    private void Awake()
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

    protected void TurnOffBullet()
    {
        _timeAfterSpaw = 0;
        gameObject.SetActive(false);
        gameObject.transform.parent = _parent;

        if (_parent != null)
            gameObject.transform.position = _parent.transform.position;
    }
}
