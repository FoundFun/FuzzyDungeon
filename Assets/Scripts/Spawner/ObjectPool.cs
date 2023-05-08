using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public abstract class ObjectPool : MonoBehaviour
{
    [Header("BaseComponents")]
    [SerializeField] private Game _game;
    [SerializeField] private Mouse _mouse;
    [SerializeField] private GameObject _containerEnemies;
    [SerializeField] private GameObject _containerPlayers;
    [SerializeField] private GameObject _containerSmokes;
    [SerializeField] private GameObject _containerBloods;
    [SerializeField] private Puck _puck;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [Header("Settings")]
    [Range(0, int.MaxValue)]
    [SerializeField] private int _capacityEnemy;

    private const int EffectsNumber = 20;

    private List<Enemy> _poolEnemies = new List<Enemy>();
    private List<Player> _poolPlayers = new List<Player>();
    private List<Smoke> _poolSmoke = new List<Smoke>();
    private List<Blood> _poolBloods = new List<Blood>();

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

    public void ResetPlayer()
    {
        foreach (var player in _poolPlayers)
        {
            player.Reset();
            player.gameObject.SetActive(false);
            DisablePlayer(player);
        }
    }

    public bool TryGetNextPlayer(int currentLevel)
    {
        if (TryGetObject(out Player nextPlayer))
        {
            nextPlayer.Reset();
            nextPlayer.SetLevel(currentLevel);

            if (_game.CurrentIndexPlayer > 0)
            {
                DisablePlayer(_poolPlayers[_game.CurrentIndexPlayer - 1]);
            }

            SetNextTarget(nextPlayer);
        }

        return _game.CurrentIndexPlayer < _poolPlayers.Count - 1;
    }

    protected void Initialize(Enemy[] enemies)
    {
        for (int i = 0; i < _capacityEnemy; i++)
        {
            int index = Random.Range(0, enemies.Length);

            Enemy template = Instantiate(enemies[index], _containerEnemies.transform);
            template.Init(_poolPlayers[_game.CurrentIndexPlayer]);
            template.gameObject.SetActive(false);

            _poolEnemies.Add(template);
        }
    }

    protected void Initialize(Player[] players)
    {
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
        if (_game.CurrentIndexPlayer < _poolPlayers.Count)
        {
            player = _poolPlayers.ElementAt(_game.CurrentIndexPlayer);
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

    private void EnablePlayer(Player player)
    {
        player.Died += _game.OnDied;
        player.GetComponent<Experience>().LevelChanged += _game.OnLevelChanged;
        player.GameOver += _game.OnGameOver;
        player.HealthChanged += _game.OnHealthBarChanged;
        player.gameObject.SetActive(true);
    }

    private void DisablePlayer(Player player)
    {
        player.gameObject.SetActive(false);
        player.GetComponent<Experience>().LevelChanged -= _game.OnLevelChanged;
        player.HealthChanged -= _game.OnHealthBarChanged;
        player.GameOver -= _game.OnGameOver;
        player.Died -= _game.OnDied;
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

            firstSpell.Init(_game.DarkFillFirstSpell);
            _game.DarkFillFirstSpell.Unlock();
        }
        else
        {
            _game.DarkFillFirstSpell.Lock();
        }

        if (player.TryGetComponent(out AttackBallTransition secondSpell))
        {
            if (player.TryGetComponent(out BallAttackPlayerState ballState))
            {
                ballState.Init(_puck, _virtualCamera);
            }

            secondSpell.Init(_game.DarkFillSecondSpell);
            _game.DarkFillSecondSpell.Unlock();
        }
        else
        {
            _game.DarkFillSecondSpell.Lock();
        }

        if (player.TryGetComponent(out AttackSoulsHarvestTransition thirdSpell))
        {
            if (player.TryGetComponent(out SoulsHarvestState soulsState))
            {
                soulsState.Init(_explosion, _poolEnemies, _virtualCamera);
            }

            thirdSpell.Init(_game.DarkFillThirdSpell);
            _game.DarkFillThirdSpell.Unlock();
        }
        else
        {
            _game.DarkFillThirdSpell.Lock();
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