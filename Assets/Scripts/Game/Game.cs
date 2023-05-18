using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image[] _playerIcon;
    [SerializeField] private StartView _startView;
    [SerializeField] private GameOverView _gameOverView;
    [SerializeField] private GameScreen _gameScreen;
    [SerializeField] private SpawnerPlayer _spawnerPlayer;
    [SerializeField] private SpawnerEnemy _spawnerEnemy;
    [SerializeField] private TMP_Text _verbalWarningText;
    [SerializeField] private TMP_Text _currentLevel;
    [SerializeField] private TMP_Text _targetLevel;
    [SerializeField] private AudioMixerSnapshot _normal;
    [SerializeField] private AudioMixerSnapshot _gameOver;
    [SerializeField] private DarkFill _darkFillFirstSpell;
    [SerializeField] private DarkFill _darkFillSecondSpell;
    [SerializeField] private DarkFill _darkFillThirdSpell;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Yandex _yandex;

    private const float DelayReset = 2;
    private const float DelayRespawn = 1;
    private const int MaxNumberVerbalWarning = 4;
    private const int StartStepLevel = 20;
    private const int StartCurrentLevel = 1;
    private const int FactorLevel = 2;
    private const string Slash = "/";
    private const string EndLevel = "???";

    private Coroutine _coroutine;
    private int _stepLevel = StartStepLevel;
    private int _numberVerbalWarning;
    private int _currentIndexPlayer;

    public DarkFill DarkFillFirstSpell => _darkFillFirstSpell;
    public DarkFill DarkFillSecondSpell => _darkFillSecondSpell;
    public DarkFill DarkFillThirdSpell => _darkFillThirdSpell;
    public int CurrentIndexPlayer => _currentIndexPlayer;

    private void Awake()
    {
        _yandex.OnShowAds();
    }

    private void OnEnable()
    {
        _startView.StartButtonClick += OnStartButtonClick;
        _gameOverView.PlayButtonClick += OnPlayButtonClick;
    }

    private void OnDisable()
    {
        _startView.StartButtonClick -= OnStartButtonClick;
        _gameOverView.PlayButtonClick -= OnPlayButtonClick;
    }

    private void Start()
    {
        Time.timeScale = 0;
        _targetLevel.text = _stepLevel.ToString();
        _numberVerbalWarning = 0;
        _currentIndexPlayer = 0;
        _currentLevel.text = StartCurrentLevel.ToString();
        _spawnerPlayer.TryGetNext(StartCurrentLevel);
        _gameOverView.CloseScreen();
        _startView.OpenScreen();
    }

    private IEnumerator Reset()
    {
        Time.timeScale = 0.3f;

        yield return new WaitForSecondsRealtime(DelayReset);

        Time.timeScale = 0;
        _currentIndexPlayer = 0;
        _numberVerbalWarning = 0;
        _stepLevel = StartStepLevel;
        _targetLevel.text = _stepLevel.ToString();
        _currentLevel.text = StartCurrentLevel.ToString();
        _verbalWarningText.text = $"{_numberVerbalWarning} {Slash} {MaxNumberVerbalWarning}";
        _gameScreen.OnStopMusic();
        _gameOverView.CloseScreen();
        _startView.OpenScreen();

        yield return new WaitForSecondsRealtime(DelayRespawn);

        _spawnerPlayer.Reset();
        _spawnerPlayer.TryGetNext(StartCurrentLevel);
        _spawnerEnemy.Reset();
        _spawnerEnemy.ResetSpawnParameters();
        DisableAllIcon();
    }

    public void OnLevelChanged(int level)
    {
        _currentLevel.text = level.ToString();
    }

    public void OnDied(Player player)
    {
        int currentLevel = GetLevel(_currentLevel);
        int targetLevel = GetLevel(_targetLevel);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        if (currentLevel < targetLevel)
        {
            if (IsGameOver())
            {
                _coroutine = StartCoroutine(Reset());
                _normal.TransitionTo(0);
            }
            else
            {
                _coroutine = StartCoroutine(OnRestart(player, currentLevel));
            }
        }
        else
        {
            _coroutine = StartCoroutine(OnNextLevel(currentLevel));
        }
    }

    public void OnHealthBarChanged(int health)
    {
        _healthBar.OnHealthChanged(health);
    }

    private int GetLevel(TMP_Text level)
    {
        bool isNumber = int.TryParse(level.text, out int result);

        return isNumber ? result : int.MaxValue;
    }

    private bool IsGameOver()
    {
        _numberVerbalWarning++;
        _verbalWarningText.text = $"{_numberVerbalWarning} {Slash} {MaxNumberVerbalWarning}";

        return _numberVerbalWarning >= MaxNumberVerbalWarning;
    }

    private void OnStartButtonClick()
    {
        Time.timeScale = 1;
        _startView.CloseScreen();
        _gameScreen.OnPlayMusic();
        EnableNextIcon();
    }

    private void OnPlayButtonClick()
    {
        Time.timeScale = 1;
        _gameOverView.CloseScreen();
        _normal.TransitionTo(0);
        EnableNextIcon();
    }

    private void EnableNextIcon()
    {
        if (_playerIcon.Length > _currentIndexPlayer && _currentIndexPlayer >= 0)
        {
            _playerIcon[_currentIndexPlayer].gameObject.SetActive(true);
        }
    }

    private void DisableAllIcon()
    {
        foreach (var icon in _playerIcon)
        {
            icon.gameObject.SetActive(false);
        }
    }

    private IEnumerator OnRestart(Player player, int currentLevel)
    {
        Time.timeScale = 0.3f;

        yield return new WaitForSecondsRealtime(DelayReset);

        Time.timeScale = 0;
        _gameOverView.OpenScreen();
        _gameOver.TransitionTo(0);

        yield return new WaitForSecondsRealtime(DelayRespawn);

        player.Reset();
        player.SetLevel(currentLevel);
        _spawnerEnemy.Reset();
    }

    private IEnumerator OnNextLevel(int currentLevel)
    {
        Time.timeScale = 0.3f;

        yield return new WaitForSecondsRealtime(DelayReset);

        Time.timeScale = 0;
        _gameOverView.OpenScreen();
        _gameOver.TransitionTo(0);

        yield return new WaitForSecondsRealtime(DelayRespawn);

        _currentIndexPlayer++;

        if (_spawnerPlayer.TryGetNext(currentLevel))
        {
            _stepLevel *= FactorLevel;
            _targetLevel.text = _stepLevel.ToString();
        }
        else
        {
            _targetLevel.text = EndLevel;
        }

        _spawnerEnemy.Reset();
    }
}