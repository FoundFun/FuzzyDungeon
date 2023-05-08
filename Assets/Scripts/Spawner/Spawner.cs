using System.Collections;
using UnityEngine;

public class Spawner : ObjectPool
{
    [Header("Components")]
    [SerializeField] private Player[] _players;
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private Smoke _smoke;
    [SerializeField] private Blood _blood;

    private const float StartMaxSecondBetweenSpawn = 7;
    private const float StartMinSecondBetweenSpawn = 1;
    private const int StartMaxFrameSpawnNumberEnemy = 6;
    private const int StartMinFrameSpawnNumberEnemy = 1;

    private float _currentSecondBetweenSpawn = 5;
    private int _maxSpreadSpawn = 2;
    private int _minSpreadSpawn = -1;
    private int _spawnAcceleration = 1;
    private int _targetSpawnedEnemies = 20;
    private bool _isSpawn = false;

    private SpawnPoint[] _spawnPoints;
    private Coroutine _coroutine;
    private float _currentMaxSecondBetweenSpawn;
    private float _currentMinSecondBetweenSpawn;
    private int _currentMaxFrameSpawnNumberEnemy;
    private int _currentMinFrameSpawnNumberEnemy;
    private int _counterSpawnedEnemies;
    private int _enemyCount;

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();

        Initialize(_players);
        Initialize(_smoke, _blood);
    }

    private void Start()
    {
        Initialize(_enemies);

        _currentMaxFrameSpawnNumberEnemy = StartMaxFrameSpawnNumberEnemy;
        _currentMinFrameSpawnNumberEnemy = StartMinFrameSpawnNumberEnemy;
        _currentMaxSecondBetweenSpawn = StartMaxSecondBetweenSpawn;
        _currentMinSecondBetweenSpawn = StartMinSecondBetweenSpawn;

        if (TryGetObject(out Player player))
        {
            player.Reset();
            player.gameObject.SetActive(true);
        }

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

            for (int i = 0; i < _enemyCount; i++)
            {
                if (TryGetObject(out Enemy enemy) && TryGetObject(out Smoke smoke))
                {
                    int index = Random.Range(0, _spawnPoints.Length);
                    int polarityX = Random.Range(_minSpreadSpawn, _maxSpreadSpawn);
                    int polarityY = Random.Range(_minSpreadSpawn, _maxSpreadSpawn);

                    Vector3 spawnPoint = new Vector3(_spawnPoints[index].transform.position.x + polarityX, _spawnPoints[index].transform.position.y + polarityY);

                    enemy.transform.position = spawnPoint;
                    enemy.gameObject.SetActive(true);
                    smoke.OnBlowUp(enemy);

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

            _currentSecondBetweenSpawn = Random.Range(_currentMinSecondBetweenSpawn, _currentMaxSecondBetweenSpawn);
            _enemyCount = Random.Range(_currentMinFrameSpawnNumberEnemy, _currentMaxFrameSpawnNumberEnemy);

            _isSpawn = false;
            _coroutine = StartCoroutine(CountTimeSpawn());
        }
    }

    public void Reset()
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
}