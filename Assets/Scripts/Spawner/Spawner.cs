using UnityEngine;

public class Spawner : ObjectPool
{
    [Header("Components")]
    [SerializeField] private Player[] _players;
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private Smoke _smoke;
    [SerializeField] private Blood _blood;
    [Header("Settings")]
    [SerializeField] private float _minSecondBetweenSpawn;
    [SerializeField] private float _maxSecondBetweenSpawn;
    [SerializeField] private int _minFrameSpawnNumberEnemy;
    [SerializeField] private int _maxFrameSpawnNumberEnemy;

    private int _maxSpreadSpawn = 2;
    private int _minSpreadSpawn = -1;
    private int _spawnAcceleration = 1;
    private int targetSpawnedEnemies = 20;

    private float _currentSecondBetweenSpawn = 5;

    private SpawnPoint[] _spawnPoints;
    private float _elapsedTime;
    private float _quantity;
    private int _counterSpawnedEnemies;

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();

        Initialize(_players);
        Initialize(_smoke, _blood);
    }

    private void Start()
    {
        Initialize(_enemies);

        if (TryGetObject(out Player player))
        {
            player.Reset();
            player.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime < _currentSecondBetweenSpawn)
        {
            return;
        }

        for (int i = 0; i < _quantity; i++)
        {
            if (TryGetObject(out Enemy enemy) && TryGetObject(out Smoke smoke))
            {
                _elapsedTime = 0;

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

        if (_counterSpawnedEnemies >= targetSpawnedEnemies)
        {
            float minValue = _maxSecondBetweenSpawn - _spawnAcceleration;

            if (_minSecondBetweenSpawn < minValue)
            {
                _maxSecondBetweenSpawn -= _spawnAcceleration;
                _maxFrameSpawnNumberEnemy += _spawnAcceleration;
                _counterSpawnedEnemies = 0;
            }
        }

        _currentSecondBetweenSpawn = Random.Range(_minSecondBetweenSpawn, _maxSecondBetweenSpawn);
        _quantity = Random.Range(_minFrameSpawnNumberEnemy, _maxFrameSpawnNumberEnemy);
    }
}