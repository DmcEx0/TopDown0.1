using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Transform _spawnPointsContainer;

    [SerializeField] private int _poolCount = 3;
    [SerializeField] private bool _isAutoExpand = false;
    [SerializeField] private Transform _spawnPoolContainer;

    private List<ObjectPool<Enemy>> _enemyPools = new List<ObjectPool<Enemy>>();
    private ObjectPool<Enemy> _currentPool;

    private int _currentPoolNumber = 0;
    private Transform[] _spawnPoints;
    private Wave _currentWave;
    private int _currentWaveNumber = 0;
    private int _spawned;

    public void StartWave()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void Start()
    {
        SetSpawnPoints();
        SetWave(_currentWaveNumber);

        for (int i = 0; i < _currentWave.EnemiesInWave.Length; i++)
        {
            _enemyPools.Add(InitializePool(i));
        }

        SetPool(_currentPoolNumber);
    }

    private ObjectPool<Enemy> InitializePool(int index)
    {
        _currentPool = new ObjectPool<Enemy>(_currentWave.EnemiesInWave[index].Template, _poolCount, _spawnPoolContainer.transform);
        _currentPool.IsAutoExpand = _isAutoExpand;

        return _currentPool;
    }

    private void SetEnemy()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Length);

        Enemy enemy = _currentPool.GetFreeElement();
        enemy.transform.position = _spawnPoints[randomIndex].position;

        enemy.Init(_player, transform);
        enemy.Dying += OnEnemyDying;
    }

    private IEnumerator SpawnEnemy()
    {
        WaitForSeconds delay = new WaitForSeconds(_currentWave.Delay);
        int enemiesIndex = 0;

        while (enemiesIndex <= _currentWave.EnemiesInWave.Length - 1)
        {
            SetEnemy();

            _spawned++;

            if (_spawned >= _currentWave.EnemiesInWave[enemiesIndex].Count)
            {
                SetPool(++_currentPoolNumber);
                enemiesIndex++;
                _spawned = 0;
            }

            yield return delay;
        }
    }

    private void SetSpawnPoints()
    {
        _spawnPoints = new Transform[_spawnPointsContainer.childCount];

        for (int i = 0; i < _spawnPointsContainer.childCount; i++)
        {
            _spawnPoints[i] = _spawnPointsContainer.GetChild(i);
        }
    }

    private void SetWave(int index)
    {
        _currentWave = _waves[index];
    }

    private void SetPool(int index)
    {
        if (index <= _enemyPools.Count - 1)
            _currentPool = _enemyPools[index];
    }

    private void OnEnemyDying(Enemy enemy)
    {
        enemy.Dying -= OnEnemyDying;

        _player.AddMoney(enemy.Reward);
    }
}

[System.Serializable]
public struct Wave
{
    public float Delay;
    public EnemiesInWave[] EnemiesInWave;
}

[System.Serializable]
public struct EnemiesInWave
{
    public Enemy Template;
    public int Count;
}