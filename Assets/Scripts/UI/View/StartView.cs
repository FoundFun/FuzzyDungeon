using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class StartView : View
{
    [SerializeField] private TMP_Text _dungeonText;
    [SerializeField] private TMP_Text _keeperText;
    [SerializeField] private TMP_Text _clickToPlayText;

    private const float TargetPositionText = -100;
    private const float DungeonTextSpeedAnimation = 4;
    private const float KeeperTextSpeedAnimation = DungeonTextSpeedAnimation + 0.1f;
    private const float LerpDuration = 100;

    private Coroutine _flickerTextCoroutine;
    private Coroutine _musicCoroutine;
    private AudioSource _audioSource;

    public event UnityAction StartButtonClick;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    public void OpenScreen()
    {
        Open();
    }

    public void CloseScreen()
    {
        Close();
    }

    protected override void Open()
    {
        base.Open();

        _dungeonText.gameObject.LeanMoveLocalX(TargetPositionText, DungeonTextSpeedAnimation)
            .setEaseInOutExpo().setIgnoreTimeScale(true);
        _keeperText.gameObject.LeanMoveLocalX(TargetPositionText, KeeperTextSpeedAnimation)
            .setEaseInOutExpo().setIgnoreTimeScale(true);

        if (_flickerTextCoroutine != null)
            StopCoroutine(_flickerTextCoroutine);

        if (_musicCoroutine != null)
            StopCoroutine(_musicCoroutine);

        _musicCoroutine = StartCoroutine(PlayMusic());
        _flickerTextCoroutine = StartCoroutine(Flicker());
    }

    protected override void Close()
    {
        base.Close();

        _dungeonText.gameObject.LeanMoveLocalX(-Screen.width, 1)
            .setEaseOutExpo().setIgnoreTimeScale(true);
        _keeperText.gameObject.LeanMoveLocalX(-Screen.width, 1)
            .setEaseOutExpo().setIgnoreTimeScale(true);

        if (_flickerTextCoroutine != null)
            StopCoroutine(_flickerTextCoroutine);

        if (_musicCoroutine != null)
            StopCoroutine(_musicCoroutine);

        _musicCoroutine = StartCoroutine(StopMusic());
    }

    protected override void OnPlayGameButtonClick()
    {
        StartButtonClick?.Invoke();
    }

    private IEnumerator Flicker()
    {
        Color targetColor = _clickToPlayText.color;
        float delayedAppearance = 3;
        float targetAlpha = 1;
        float speed = 3;

        yield return new WaitForSecondsRealtime(delayedAppearance);

        while (true)
        {
            targetColor.a = Mathf.MoveTowards(targetColor.a, targetAlpha,
                speed * Time.unscaledDeltaTime);
            _clickToPlayText.color = targetColor;

            if (targetColor.a == targetAlpha)
            {
                targetAlpha = targetAlpha == 1 ? 0 : 1;
            }

            yield return null;
        }
    }

    private IEnumerator PlayMusic()
    {
        float elapsed = 0;
        float targetValue = 0.5f;

        _audioSource.Play();

        while (_audioSource.volume != targetValue)
        {
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, targetValue,
                elapsed / LerpDuration);
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }

    private IEnumerator StopMusic()
    {
        float elapsed = 0;
        float targetValue = 0;

        while (_audioSource.volume != targetValue)
        {
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, targetValue,
                elapsed / LerpDuration);
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        _audioSource.Stop();
    }
}