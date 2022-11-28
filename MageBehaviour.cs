using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : EnemyBehaviour
{
    [SerializeField] private EnemyBullet _bulletTemplate;
    [SerializeField] private Transform _shootPoint;

    [SerializeField] private Transform _spawnPoolContainer;
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = false;

    private MageEnemy _mageEnemy;
    private ObjectPool<EnemyBullet> _bulletPool;

    public void MageAttacked() => SetBullet();


    private void Awake()
    {
        _mageEnemy = GetComponent<MageEnemy>();
        InitializePool();
    }

    private void InitializePool()
    {
        _bulletPool = new ObjectPool<EnemyBullet>(_bulletTemplate, _poolCount, _spawnPoolContainer.transform);
        _bulletPool.IsAutoExpand = _isAutoExpand;
    }

    private void SetBullet()
    {
        EnemyBullet bulletEnemy = _bulletPool.GetFreeElement();
        bulletEnemy.transform.position = _shootPoint.position;
        bulletEnemy.transform.parent = null;

        bulletEnemy.Init(Target.gameObject, _spawnPoolContainer.transform);
        bulletEnemy.SetDamage(_mageEnemy.Damage);
    }
}
