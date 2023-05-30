using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Players;

public class SpawnerPlayer : ObjectPool<Player>
{
    [SerializeField] private Player[] _players;
    [SerializeField] private Game _game;
    [SerializeField] private SpawnerEnemy _spawnerEnemy;
    [SerializeField] private GameObject _containerPlayers;
    [SerializeField] private Mouse _mouse;
    [SerializeField] private Puck _puck;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private List<Player> _pool;

    public Player CurrentPlayer => _pool[_game.CurrentIndexPlayer];

    private void Awake()
    {
        _pool = Initialize(_players, _containerPlayers);
        InitMouse(_pool);
        InitSpells(_pool);
    }

    public void Reset()
    {
        foreach (var player in _pool)
        {
            player.Reset();
            DisablePlayer(player);
        }
    }

    public bool TryGetNext(int currentLevel)
    {
        if (TryGetObject(out Player nextPlayer, _game.CurrentIndexPlayer))
        {
            nextPlayer.Reset();
            nextPlayer.SetLevel(currentLevel);
            EnablePlayer(nextPlayer);
            SetCurrentSpells(nextPlayer);

            if (_game.CurrentIndexPlayer > 0)
                DisablePlayer(_pool[_game.CurrentIndexPlayer - 1]);
        }

        return _game.CurrentIndexPlayer < _pool.Count - 1;
    }

    private void InitMouse(List<Player> pool)
    {
        foreach (var player in pool)
            player.Init(_mouse);
    }

    private void EnablePlayer(Player player)
    {
        player.Died += _game.OnDied;
        player.GetComponent<Experience>().LevelChanged += _game.OnLevelChanged;
        player.HealthChanged += _game.OnHealthBarChanged;
        player.gameObject.SetActive(true);
    }

    private void DisablePlayer(Player player)
    {
        player.gameObject.SetActive(false);
        player.GetComponent<Experience>().LevelChanged -= _game.OnLevelChanged;
        player.HealthChanged -= _game.OnHealthBarChanged;
        player.Died -= _game.OnDied;
    }

    private void SetCurrentSpells(Player player)
    {
        if (player.GetComponent<AttackAirTransition>())
            _game.DarkFillFirstSpell.Unlock();
        else
            _game.DarkFillFirstSpell.Lock();

        if (player.GetComponent<BallAttackState>())
            _game.DarkFillSecondSpell.Unlock();
        else
            _game.DarkFillSecondSpell.Lock();

        if (player.GetComponent<SoulsHarvestState>())
            _game.DarkFillThirdSpell.Unlock();
        else
            _game.DarkFillThirdSpell.Lock();
    }

    private void InitSpells(List<Player> players)
    {
        foreach (var player in players)
        {
            if (player.TryGetComponent(out AirAttackState airState))
            {
                airState.Init(_explosion, _virtualCamera);

                if (player.TryGetComponent(out AttackAirTransition firstSpell))
                    firstSpell.Init(_game.DarkFillFirstSpell);
            }

            if (player.TryGetComponent(out BallAttackState ballState))
            {
                ballState.Init(_puck, _virtualCamera);

                if (player.TryGetComponent(out AttackBallTransition secondSpell))
                    secondSpell.Init(_game.DarkFillSecondSpell);
            }

            if (player.TryGetComponent(out SoulsHarvestState soulsState))
            {
                soulsState.Init(_explosion, _spawnerEnemy.OnKillAllActive, _virtualCamera);

                if (player.TryGetComponent(out AttackSoulsHarvestTransition thirdSpell))
                    thirdSpell.Init(_game.DarkFillThirdSpell);
            }
        }
    }
}