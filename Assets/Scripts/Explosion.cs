using System.Collections;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    private const float Delay = 1;
    private const float SpawnPositionY = 1;
    private const float IntensityShaking = 1;

    private Coroutine _coroutine;
    private AudioSource _explosionAudio;
    private CinemachineBasicMultiChannelPerlin _virtualCamera;

    private void Awake()
    {
        _explosionAudio = GetComponent<AudioSource>();
        _explosionAudio.volume = 0.5f;
        _explosionAudio.playOnAwake = true;
    }

    public void Init(CinemachineVirtualCamera virtualCamera)
    {
        _virtualCamera = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _virtualCamera.m_AmplitudeGain = 0;
    }

    public void OnBlowUp(Player player)
    {
        Vector2 targetPosition = new Vector2(player.transform.position.x, player.transform.position.y - SpawnPositionY);
        gameObject.transform.position = targetPosition;
        gameObject.SetActive(true);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(BlowUp());
    }

    private IEnumerator BlowUp()
    {
        float elapsedTime = 0;

        while (elapsedTime < Delay)
        {
            _virtualCamera.m_AmplitudeGain = IntensityShaking;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _virtualCamera.m_AmplitudeGain = 0;
        gameObject.SetActive(false);
    }
}