using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmemySpawner : MonoBehaviour
{
    public event Action<int,int> EnemiesCountChange; 

    [SerializeField] private Player _player;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private EnemyPool _pool;
    [SerializeField] private float _spawnHight = 5.0f;


    public List<Wave> Waves => _waves;

    private static System.Random _rand = new System.Random();
    private Coroutine _doSpawn;
    private Vector3 _randSpawnPoint => new Vector3(_rand.Next(20,80),_spawnHight,_rand.Next(20,80));
    private int _countInWave => _waves[_curWave].EnemyCount;
    private int _maxEnemyInScene => _waves[_curWave].CountInScene;
    private WaitForSeconds _cicleDelay = new WaitForSeconds(1f);
    private int _enemyInScene;
    private int _enemyDie;
    private float _spawnDistance = 30f;
    private int _curWave =-1;
        
    private void Start()
    {   
        GoNextWave();
        EnemiesCountChange?.Invoke(_curWave, _countInWave - _enemyDie);

        StartCoroutine(DoSpawnCicle());
    }

    private IEnumerator DoSpawnCicle()
    {
        if (_enemyInScene < _maxEnemyInScene && _enemyInScene + _enemyDie <= _countInWave)
            SpawnNewEnemy();
            
        yield return _cicleDelay;
        StartCoroutine(DoSpawnCicle());

    }

    private void SpawnNewEnemy() =>_doSpawn = StartCoroutine(DoSpawnNewEnemy());

    public void OnEnemyDie(Enemy sender)
    {
        _enemyInScene--;
        _enemyDie++;

        if (_enemyDie >= _countInWave)
            GoNextWave();

        EnemiesCountChange?.Invoke(_curWave, _countInWave - _enemyDie);
    }

    private void GoNextWave()
    {
        _enemyDie = 0;
        _curWave++;

        if (_pool == null)
        {
            _pool = new GameObject("Pool").AddComponent<EnemyPool>();
            _pool.Init(_waves[_curWave], _player, this);
        }
        else
        { 
            _pool.ReBoot(_waves[_curWave], _player);
        }
    }

    private IEnumerator DoSpawnNewEnemy()
    {
        if (_doSpawn != null)
            StopCoroutine(_doSpawn);

        bool isSucses = false;

        while (isSucses == false)
        {
            isSucses = _pool.TryGetRandomEnemy(out Enemy enemy);
            Vector3 newSpawnPoint;

            if (isSucses)
            {
                do
                    newSpawnPoint = _randSpawnPoint;
                while (Vector3.Distance(_player.transform.position, newSpawnPoint) < _spawnDistance);

                enemy.transform.position = newSpawnPoint;
                _enemyInScene++;
                enemy.gameObject.SetActive(true);
                yield return null;
            }
        }
    }
}