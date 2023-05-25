using System.Collections;
using UnityEngine;

public abstract class Effect<T> : MonoBehaviour where T : MonoBehaviour
{
    [Range(0, MaxTimeAlive)]
    [SerializeField] protected float _timeAlive;

    private const float MaxTimeAlive = 2;

    private Coroutine _coroutine;

    private void OnValidate()
    {
        _timeAlive = Mathf.Clamp(_timeAlive, 0, MaxTimeAlive);
    }

    public void Play(T targetObject, float height = 0)
    {
        gameObject.transform.position = new Vector2(targetObject.transform.position.x, targetObject.transform.position.y + height);
        gameObject.gameObject.SetActive(true);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(_timeAlive);

        gameObject.SetActive(false);
    }
}
