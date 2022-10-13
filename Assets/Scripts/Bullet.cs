using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

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

    private void Update()
    {
        _timeAfterSpaw += Time.deltaTime;

        if (_timeAfterSpaw >= _lifeTime)
        {
            gameObject.SetActive(false);
            gameObject.transform.parent = _parent;
            _timeAfterSpaw = 0;
        }
    }

    private void FixedUpdate()
    {
        float speed = _speed * Time.fixedDeltaTime;

        transform.Translate(_direction.normalized * speed, Space.World);
    }
}
