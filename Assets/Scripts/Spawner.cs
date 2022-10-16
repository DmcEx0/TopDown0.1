using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : ObjectPool
{
    [SerializeField] private Transform _spawnPointsContainer;
    [SerializeField] private List<Wave> _waves;

    [SerializeField] private Player _player;

    private Transform[] _spawnPoints;
    private Wave _currentWave;
    private int _currentWaveNumber = 0;
    private int _spawned;

    private void Start()
    {
        SetSpawnPoints();
        SetWave(_currentWaveNumber);
        Initialize(_currentWave.Template);
        StartCoroutine(SpawnEnemy());
    }

    private void SetEnemy(GameObject enemy)
    {
        int randomIndex = Random.Range(0, _spawnPoints.Length);

        enemy.SetActive(true);
        enemy.transform.position = _spawnPoints[randomIndex].position;

        Enemy enemyTemplate = enemy.GetComponent<Enemy>();
        enemyTemplate.Init(_player, Container.transform);
        enemyTemplate.Dying += OnEnemyDying;
    }

    private IEnumerator SpawnEnemy()
    {
        WaitForSeconds delay = new WaitForSeconds(_currentWave.Delay);

        while (_spawned <= _currentWave.Count - 1)
        {
            if (TryGetObject(out GameObject enemy))
            {
                SetEnemy(enemy);

                _spawned++;
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

    private void OnEnemyDying(Enemy enemy)
    {
        enemy.Dying -= OnEnemyDying;

        _player.AddMoney(enemy.Reward);
    }
}

[System.Serializable]
public class Wave
{
    public GameObject Template;
    public float Delay;
    public int Count;
}
