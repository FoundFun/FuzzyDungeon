using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class Game : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StartView _startView;
    [SerializeField] private GameOverView _gameOverView;
    [SerializeField] private GameScreen _gameScreen;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private TMP_Text _verbalWarningText;
    [SerializeField] private AudioMixerSnapshot _normal;
    [SerializeField] private AudioMixerSnapshot _gameOver;

    private const int MaxNumberVerbalWarning = 4;
    private const int StartNumberVerbalWarning = 0;
    private const string Slash = "/";

    private int _numberVerbalWarning;

    public int MaxValueVerbalWarning => MaxNumberVerbalWarning;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _numberVerbalWarning = StartNumberVerbalWarning;
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
        _gameOverView.CloseScreen();
        _startView.OpenScreen();
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        _gameOverView.OpenScreen();
        _gameOver.TransitionTo(0);
    }

    public int IncreaseVerbalWarning()
    {
        _numberVerbalWarning++;

        if (_numberVerbalWarning >= MaxNumberVerbalWarning)
        {
            Reset();
            _normal.TransitionTo(0);
            _gameScreen.OnStopMusic();
        }

        _verbalWarningText.text = _numberVerbalWarning.ToString() + Slash + MaxNumberVerbalWarning.ToString();

        return _numberVerbalWarning;
    }

    private void OnStartButtonClick()
    {
        Time.timeScale = 1;
        _startView.CloseScreen();
        _gameScreen.OnPlayMusic();
    }

    private void OnPlayButtonClick()
    {
        Time.timeScale = 1;
        _gameOverView.CloseScreen();
        _normal.TransitionTo(0);
    }

    private void Reset()
    {
        Time.timeScale = 0;
        _numberVerbalWarning = StartNumberVerbalWarning;
        _gameOverView.CloseScreen();
        _startView.OpenScreen();
        _spawner.ReserPoolPlayer();
        _spawner.ResetPoolEnemy();
    }
}