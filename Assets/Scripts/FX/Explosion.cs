using UnityEngine;
using Cinemachine;
using Players;
using Enemies;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class Explosion : Effect<Player>
{
    private const float IntensityShaking = 1;

    private Demon _demon;
    private AudioSource _explosionAudio;
    private CinemachineBasicMultiChannelPerlin _virtualCamera;

    private void Awake()
    {
        _explosionAudio = GetComponent<AudioSource>();
        _explosionAudio.volume = 0.5f;
        _explosionAudio.playOnAwake = true;
    }

    private void OnEnable()
    {
        if (_virtualCamera != null)
            _virtualCamera.m_AmplitudeGain = IntensityShaking;
    }

    private void OnDisable()
    {
        if (_virtualCamera != null)
            _virtualCamera.m_AmplitudeGain = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.OnDie();
            _demon.AddExperience(enemy.Experience);
        }
    }

    public void Init(Demon demon,CinemachineVirtualCamera virtualCamera)
    {
        _demon = demon;
        _virtualCamera = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _virtualCamera.m_AmplitudeGain = 0;
    }
}