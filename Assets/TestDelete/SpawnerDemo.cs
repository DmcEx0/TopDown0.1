using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDemo : MonoBehaviour
{
    //!!!!!!!
    [SerializeField] private Transform _spawnPointsContainer;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Player _player;

    [SerializeField] private List<EnemiesInWave> _enemies;

    private Transform[] _spawnPoints;
    private Wave _currentWave;
    private int _currentWaveNumber = 0;
    private int _spawned;

    public void StartWave()
    {
        //StartCoroutine(SpawnEnemy());
    }

    private void Start()
    {
        SetSpawnPoints();
        SetWave(_currentWaveNumber);

        //foreach (var templates in _currentWave.Enemies)
        //{
        //    Initialize(templates.Template);
        //}
    }

    private void SetEnemy(GameObject enemyTemplate)
    {
        int randomIndex = Random.Range(0, _spawnPoints.Length);

        enemyTemplate.SetActive(true);
        enemyTemplate.transform.position = _spawnPoints[randomIndex].position;

        Enemy enemy = enemyTemplate.GetComponent<Enemy>();
        //enemy.Init(_player, Container.transform);
        enemy.Dying += OnEnemyDying;
    }

    //private IEnumerator SpawnEnemy()
    //{
    //    WaitForSeconds delay = new WaitForSeconds(_currentWave.Delay);

    //    while (/*_spawned <= _currentWave.Count - 1*/ true)
    //    {
    //        if (TryGetObject(out GameObject enemy))
    //        {
    //            for (int i = 0; i < _currentWave.Enemies.Length; i++)
    //            {
    //                for (int j = 0; j < _currentWave.Enemies[i].Count; j++)
    //                {
    //                    SetEnemy(enemy);
    //                }
    //            }

    //            _spawned++;
    //        }

    //        yield return delay;
    //    }
    //}

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

//[System.Serializable]
//public class Wave
//{
//    public float Delay;
//    public Enemies[] Enemies;
//}

//[System.Serializable]
//public class Enemies
//{
//    public Enemy Template;
//    public int Count;
//}
