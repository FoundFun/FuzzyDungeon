using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerEnemy : ObjectPool<Enemy>
{
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private GameObject _containerEnemies;
    [SerializeField] private SpawnerPlayer _spawnerPlayer;
    [SerializeField] private SpawnerSmoke _spawnerSmoke;
    [SerializeField] private SpawnerBlood _spawnerBlood;

    private const float StartMaxSecondBetweenSpawn = 10;
    private const float StartMinSecondBetweenSpawn = 2;
    private const int StartMaxFrameSpawnNumberEnemy = 3;
    private const int StartMinFrameSpawnNumberEnemy = 1;

    private float _currentSecondBetweenSpawn = 5;
    private int _maxSpreadSpawn = 2;
    private int _minSpreadSpawn = -1;
    private int _spawnAcceleration = 1;
    private int _targetSpawnedEnemies = 5;
    private bool _isSpawn = false;

    private SpawnPoint[] _spawnPoints;
    private Coroutine _coroutine;
    private float _currentMaxSecondBetweenSpawn;
    private float _currentMinSecondBetweenSpawn;
    private int _currentMaxFrameSpawnNumberEnemy;
    private int _currentMinFrameSpawnNumberEnemy;
    private int _counterSpawnedEnemies;
    private int _randomIndex;
    private int _count;

    private List<Enemy> _pool = new List<Enemy>();

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    private void Start()
    {
        _pool = Initialize(_enemies, _containerEnemies);

        _currentMaxFrameSpawnNumberEnemy = StartMaxFrameSpawnNumberEnemy;
        _currentMinFrameSpawnNumberEnemy = StartMinFrameSpawnNumberEnemy;
        _currentMaxSecondBetweenSpawn = StartMaxSecondBetweenSpawn;
        _currentMinSecondBetweenSpawn = StartMinSecondBetweenSpawn;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(CountTimeSpawn());
    }

    private void Update()
    {
        if (_isSpawn)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            for (int i = 0; i < _count; i++)
            {
                _randomIndex = Random.Range(0, _pool.Count);

                if (TryGetObject(out Enemy enemy, _randomIndex))
                {
                    int index = Random.Range(0, _spawnPoints.Length);
                    int polarityX = Random.Range(_minSpreadSpawn, _maxSpreadSpawn);
                    int polarityY = Random.Range(_minSpreadSpawn, _maxSpreadSpawn);

                    Vector3 spawnPoint = new Vector3(_spawnPoints[index].transform.position.x + polarityX,
                        _spawnPoints[index].transform.position.y + polarityY);

                    enemy.Init(_spawnerPlayer.CurrentPlayer);
                    enemy.transform.position = spawnPoint;
                    enemy.gameObject.SetActive(true);
                    _spawnerSmoke.OnSpawned(enemy);
                    enemy.Died += OnDied;

                    _counterSpawnedEnemies++;
                }
            }

            if (_counterSpawnedEnemies >= _targetSpawnedEnemies)
            {
                float minValue = _currentMaxSecondBetweenSpawn - _spawnAcceleration;

                if (_currentMinSecondBetweenSpawn < minValue)
                {
                    _currentMaxSecondBetweenSpawn -= _spawnAcceleration;
                    _currentMaxFrameSpawnNumberEnemy += _spawnAcceleration;
                    _counterSpawnedEnemies = 0;
                }
            }

            _currentSecondBetweenSpawn = Random.Range(_currentMinSecondBetweenSpawn,
                _currentMaxSecondBetweenSpawn);
            _count = Random.Range(_currentMinFrameSpawnNumberEnemy,
                _currentMaxFrameSpawnNumberEnemy);

            _isSpawn = false;
            _coroutine = StartCoroutine(CountTimeSpawn());
        }
    }

    public void Reset()
    {
        foreach (var enemy in _pool)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    public void OnKillAllActive()
    {
        var activeEnemies = _pool.Where(enemy => enemy.gameObject.activeSelf == true);

        foreach (var enemy in activeEnemies)
        {
            enemy.OnDie();
        }
    }

    public void ResetSpawnParameters()
    {
        _currentMaxFrameSpawnNumberEnemy = StartMaxFrameSpawnNumberEnemy;
        _currentMinFrameSpawnNumberEnemy = StartMinFrameSpawnNumberEnemy;
        _currentMaxSecondBetweenSpawn = StartMaxSecondBetweenSpawn;
        _currentMinSecondBetweenSpawn = StartMinSecondBetweenSpawn;
    }

    private IEnumerator CountTimeSpawn()
    {
        float elapsedTime = 0;

        while (!_isSpawn)
        {
            if (elapsedTime > _currentSecondBetweenSpawn)
            {
                _isSpawn = true;
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    private void OnDied(Enemy enemy)
    {
        _spawnerBlood.OnDied(enemy);
        enemy.Died -= OnDied;
    }
}