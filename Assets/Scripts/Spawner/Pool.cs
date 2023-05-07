using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public abstract class Pool : MonoBehaviour
{
    [Header("BaseComponents")]
    [SerializeField] private Image[] _playerIcon;
    [SerializeField] private Game _game;
    [SerializeField] private Mouse _mouse;
    [SerializeField] private TMP_Text _currentLevel;
    [SerializeField] private TMP_Text _targetLevel;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _containerEnemies;
    [SerializeField] private GameObject _containerPlayers;
    [SerializeField] private GameObject _containerSmokes;
    [SerializeField] private GameObject _containerBloods;
    [SerializeField] private DarkFill _darkFillFirstSpell;
    [SerializeField] private DarkFill _darkFillSecondSpell;
    [SerializeField] private DarkFill _darkFillThirdSpell;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Puck _puck;
    [Header("Settings")]
    [Range(0, int.MaxValue)]
    [SerializeField] private int _capacityEnemy;

    private const int EffectsNumber = 20;
    private const int StartStepLevel = 10;

    private List<Enemy> _poolEnemies = new List<Enemy>();
    private List<Player> _poolPlayers = new List<Player>();
    private List<Smoke> _poolSmoke = new List<Smoke>();
    private List<Blood> _poolBloods = new List<Blood>();
    private int _stepLevel = StartStepLevel;

    private int _currentLevelPlayer;
    private int _countGameOver;

    private void OnValidate()
    {
        _capacityEnemy = Mathf.Clamp(_capacityEnemy, 0, int.MaxValue);
    }

    public void ResetEnemy()
    {
        foreach (var enemy in _poolEnemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    public void ReserPlayer()
    {
        _currentLevelPlayer = 0;
        _stepLevel = StartStepLevel;
        _targetLevel.text = _stepLevel.ToString();

        foreach (var player in _poolPlayers)
        {
            player.gameObject.SetActive(false);
            DisablePlayer(player);
        }

        SetNextTarget(_poolPlayers[_currentLevelPlayer]);
        EnablePlayer(_poolPlayers[_currentLevelPlayer]);
        _poolPlayers[_currentLevelPlayer].Reset();
        _poolPlayers[_currentLevelPlayer].gameObject.SetActive(true);
    }

    protected void Initialize(Enemy[] enemies)
    {
        for (int i = 0; i < _capacityEnemy; i++)
        {
            int index = Random.Range(0, enemies.Length);

            Enemy template = Instantiate(enemies[index], _containerEnemies.transform);
            template.Init(_poolPlayers[_currentLevelPlayer]);
            template.gameObject.SetActive(false);

            _poolEnemies.Add(template);
        }
    }

    protected void Initialize(Player[] players)
    {
        _targetLevel.text = _stepLevel.ToString();

        for (int i = 0; i < players.Length; i++)
        {
            Player template = Instantiate(players[i], _containerPlayers.transform);
            template.Init(_mouse);
            template.gameObject.SetActive(false);

            _poolPlayers.Add(template);
        }
    }

    protected void Initialize(Smoke smoke, Blood blood)
    {
        _targetLevel.text = _stepLevel.ToString();

        for (int i = 0; i < EffectsNumber; i++)
        {
            Smoke smokeTemplate = Instantiate(smoke, _containerSmokes.transform);
            smokeTemplate.gameObject.SetActive(false);

            Blood bloodTemplate = Instantiate(blood, _containerBloods.transform);
            bloodTemplate.gameObject.SetActive(false);

            _poolSmoke.Add(smokeTemplate);
            _poolBloods.Add(bloodTemplate);
        }
    }

    protected bool TryGetObject(out Enemy enemy)
    {
        enemy = _poolEnemies.FirstOrDefault(firstEnemy => firstEnemy.gameObject.activeSelf == false);

        if (enemy != null)
        {
            enemy.Died += OnDiedEneny;
        }

        return enemy != null;
    }

    protected bool TryGetObject(out Player player)
    {
        if (_currentLevelPlayer > 0)
        {
            _playerIcon[_currentLevelPlayer - 1].gameObject.SetActive(false);
        }

        _playerIcon[_currentLevelPlayer].gameObject.SetActive(true);

        if (_currentLevelPlayer < _poolPlayers.Count - 1)
        {
            player = _poolPlayers.ElementAt(_currentLevelPlayer);

            _stepLevel *= 2;
            _targetLevel.text = _stepLevel.ToString();
            _currentLevelPlayer++;
        }
        else if (_countGameOver < _game.MaxValueVerbalWarning)
        {
            player = _poolPlayers.ElementAt(_currentLevelPlayer);

            _targetLevel.text = "???";
        }
        else
        {
            player = null;
        }

        if (player != null)
        {
            UnlockSpells(player);
            EnablePlayer(player);
        }

        return player != null;
    }

    protected bool TryGetObject(out Smoke smoke)
    {
        smoke = _poolSmoke.FirstOrDefault(firstEnemy => firstEnemy.gameObject.activeSelf == false);

        return smoke != null;
    }

    protected bool TryGetObject(out Blood blood)
    {
        blood = _poolBloods.FirstOrDefault(firstEnemy => firstEnemy.gameObject.activeSelf == false);

        return blood != null;
    }

    private void OnDied(Player player)
    {
        int currentLevel = GetLevel(_currentLevel);
        int targetLevel = GetLevel(_targetLevel);

        if (currentLevel >= targetLevel && TryGetObject(out Player nextPlayer))
        {
            nextPlayer.Reset();
            nextPlayer.SetLevel(currentLevel);
            nextPlayer.gameObject.SetActive(true);

            DisablePlayer(player);
            SetNextTarget(nextPlayer);
        }
        else
        {
            _countGameOver = _game.IncreaseVerbalWarning();

            player.Reset();

            if (_countGameOver < _game.MaxValueVerbalWarning)
            {
                player.SetLevel(currentLevel);
            }
        }

        ResetEnemy();
    }

    private void OnLevelChanged(int level)
    {
        int currentLevel = int.Parse(_currentLevel.text);
        _currentLevel.text = level.ToString();
    }

    private void EnablePlayer(Player player)
    {
        player.Died += OnDied;
        player.GetComponent<Experience>().LevelChanged += OnLevelChanged;
        player.GameOver += _game.OnGameOver;
        player.HealthChanged += _healthBar.OnHealthChanged;
    }

    private void DisablePlayer(Player player)
    {
        player.gameObject.SetActive(false);
        player.GetComponent<Experience>().LevelChanged -= OnLevelChanged;
        player.HealthChanged -= _healthBar.OnHealthChanged;
        player.GameOver -= _game.OnGameOver;
        player.Died -= OnDied;
    }

    private int GetLevel(TMP_Text level)
    {
        bool isNumber = int.TryParse(level.text, out int result);

        if (isNumber)
        {
            return result;
        }
        else
        {
            return int.MaxValue;
        }
    }

    private void SetNextTarget(Player nextPlayer)
    {
        foreach (var enemy in _poolEnemies)
        {
            enemy.Init(nextPlayer);
        }
    }

    private void UnlockSpells(Player player)
    {
        if (player.TryGetComponent(out AttackAirTransition firstSpell))
        {
            if (player.TryGetComponent(out AirAttackPlayerState airState))
            {
                airState.Init(_explosion, _virtualCamera);
            }

            firstSpell.Init(_darkFillFirstSpell);
            _darkFillFirstSpell.Unlock();
        }

        if (player.TryGetComponent(out AttackBallTransition secondSpell))
        {
            if (player.TryGetComponent(out BallAttackPlayerState ballState))
            {
                ballState.Init(_puck, _virtualCamera);
            }

            secondSpell.Init(_darkFillSecondSpell);
            _darkFillSecondSpell.Unlock();
        }

        if (player.TryGetComponent(out AttackSoulsHarvestTransition thirdSpell))
        {
            if (player.TryGetComponent(out SoulsHarvestState soulsState))
            {
                soulsState.Init(_explosion, _poolEnemies, _virtualCamera);
            }

            thirdSpell.Init(_darkFillThirdSpell);
            _darkFillThirdSpell.Unlock();
        }
    }

    private void OnDiedEneny(Enemy enemy)
    {
        if (TryGetObject(out Blood blood))
        {
            blood.OnDied(enemy);
        }

        enemy.Died -= OnDiedEneny;
    }
}