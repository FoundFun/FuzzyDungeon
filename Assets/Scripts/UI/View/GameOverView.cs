using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameOverView : View
{
    [Header("Components")]
    [SerializeField] private TMP_Text _currentLevel;
    [SerializeField] private TMP_Text _gameOverLevel;

    private const string CurrentLevelText = "CURRENT LEVEL ";

    private Coroutine _coroutine;

    public event UnityAction PlayButtonClick;

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

        _gameOverLevel.text = CurrentLevelText + _currentLevel.text;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Flicker());
    }

    protected override void Close()
    {
        base.Close();

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    protected override void OnPlayGameButtonClick()
    {
        PlayButtonClick?.Invoke();
    }

    private IEnumerator Flicker()
    {
        Color targetColor = _gameOverLevel.color;
        float delayedAppearance = 1;
        float targetAlpha = 1;
        float speed = 3;

        yield return new WaitForSecondsRealtime(delayedAppearance);

        while (true)
        {
            targetColor.a = Mathf.MoveTowards(targetColor.a, targetAlpha,
                speed * Time.unscaledDeltaTime);
            _gameOverLevel.color = targetColor;

            if (targetColor.a == targetAlpha)
                targetAlpha = targetAlpha == 1 ? 0 : 1;

            yield return null;
        }
    }
}