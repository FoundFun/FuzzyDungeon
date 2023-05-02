using System.Collections;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    private const float Delay = 1.7f;

    private Coroutine _coroutine;

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    public void OnBlowUp(Enemy enemy)
    {
        gameObject.transform.position = enemy.transform.position;
        gameObject.SetActive(true);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(BlowUp());
    }

    private IEnumerator BlowUp()
    {
        yield return new WaitForSecondsRealtime(Delay);

        gameObject.SetActive(false);
    }
}