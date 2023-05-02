using System.Collections;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private const float Delay = 0.65f;

    private Coroutine _coroutine;

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    public void OnDied(Enemy enemy)
    {
        gameObject.transform.position = enemy.transform.position;
        gameObject.SetActive(true);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Died());
    }

    private IEnumerator Died()
    {

        yield return new WaitForSecondsRealtime(Delay);

        gameObject.SetActive(false);
    }
}