using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class View : MonoBehaviour
{
    [SerializeField] protected CanvasGroup CanvasGroup;
    [SerializeField] protected Button PlayButton;
    [SerializeField] private Image _loadUp;
    [SerializeField] private Image _loadDown;
    [SerializeField] private AudioSource _loadAudio;

    private const float PositionLoadingImage = 0.5f;
    private const float TimeAnimation = 0.2f;
    private const float Delay = 1;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        PlayButton.onClick.AddListener(OnPlayGameButtonClick);
    }

    private void OnDisable()
    {
        PlayButton.onClick.RemoveListener(OnPlayGameButtonClick);
    }

    protected abstract void OnPlayGameButtonClick();

    protected virtual void Open()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(PlayTransitionScene());

        CanvasGroup.LeanAlpha(1, 1).setIgnoreTimeScale(true);
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }

    protected virtual void Close()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(PlayTransitionScene());

        CanvasGroup.LeanAlpha(0, 1).setIgnoreTimeScale(true);
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator PlayTransitionScene()
    {
        _loadUp.transform.LeanMoveLocalY(-PositionLoadingImage, TimeAnimation)
            .setEaseInCirc().setIgnoreTimeScale(true);
        _loadDown.transform.LeanMoveLocalY(PositionLoadingImage, TimeAnimation)
            .setEaseInCirc().setIgnoreTimeScale(true).setOnComplete(PlayAudio);

        yield return new WaitForSecondsRealtime(Delay);

        PlayAudio();
        _loadUp.transform.LeanMoveLocalY(Screen.height, TimeAnimation)
            .setEaseInCirc().setIgnoreTimeScale(true);
        _loadDown.transform.LeanMoveLocalY(-Screen.height, TimeAnimation)
            .setEaseInCirc().setIgnoreTimeScale(true);
    }

    private void PlayAudio()
    {
        _loadAudio.Play();
    }
}