using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameScreen : MonoBehaviour
{
    private const float LerpDuration = 100;

    private AudioSource _backgroundAudio;
    private Coroutine _coroutinePlayMusic;
    private Coroutine _coroutineStopMusic;

    private void Start()
    {
        _backgroundAudio = GetComponent<AudioSource>();
    }

    public void OnPlayMusic()
    {
        if (_coroutineStopMusic != null)
        {
            StopCoroutine(_coroutineStopMusic);
        }

        if (_coroutinePlayMusic != null)
        {
            StopCoroutine(_coroutinePlayMusic);
        }

        _coroutinePlayMusic = StartCoroutine(PlayMusic());
    }

    public void OnStopMusic()
    {
        if (_coroutinePlayMusic != null)
        {
            StopCoroutine(_coroutinePlayMusic);
        }

        if (_coroutineStopMusic != null)
        {
            StopCoroutine(_coroutineStopMusic);
        }

        StartCoroutine(StopMusic());
    }

    private IEnumerator PlayMusic()
    {
        float elapsed = 0;
        float targetValue = 0.5f;

        _backgroundAudio.Play();

        while (_backgroundAudio.volume != targetValue)
        {
            _backgroundAudio.volume = Mathf.Lerp(_backgroundAudio.volume, targetValue, elapsed / LerpDuration);
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }

    private IEnumerator StopMusic()
    {
        float elapsed = 0;
        float targetValue = 0;

        while (_backgroundAudio.volume != targetValue)
        {
            _backgroundAudio.volume = Mathf.Lerp(_backgroundAudio.volume, targetValue, elapsed / LerpDuration);
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        _backgroundAudio.Stop();
    }
}