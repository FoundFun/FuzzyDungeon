using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Demon))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class SoulsHarvestState : PlayerState
{
    [SerializeField] private float _propagationSpeed;

    private const float IntensityShaking = 2;

    private readonly int IsSoulsHarvest = Animator.StringToHash("IsSoulsHarvest");

    private List<Enemy> _enemies = new List<Enemy>();

    private Demon _demon;
    private Animator _animator;
    private Coroutine _coroutine;
    private AudioSource _explosionAudio;
    private CinemachineBasicMultiChannelPerlin _virtualCamera;

    private void Awake()
    {
        _demon = GetComponent<Demon>();
        _animator = GetComponent<Animator>();
        _explosionAudio = GetComponent<AudioSource>();
        _explosionAudio.volume = 0.5f;
    }

    private void OnEnable()
    {
        _demon.SetAttackState(true);
        _coroutine = StartCoroutine(Attack());
    }
    private void OnDisable()
    {
        _demon.SetAttackState(false);
        StopCoroutine(_coroutine);
    }

    public void Init(List<Enemy> enemies, CinemachineVirtualCamera virtualCamera)
    {
        _enemies = enemies;
        _virtualCamera = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _virtualCamera.m_AmplitudeGain = 0;
    }

    private IEnumerator Attack()
    {
        _animator.SetBool(IsSoulsHarvest, true);
        _explosionAudio.Play();
        _virtualCamera.m_AmplitudeGain = IntensityShaking;

        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i].gameObject.activeSelf == true)
            {
                _enemies[i].Die();
            }

            yield return null;
        }

        _virtualCamera.m_AmplitudeGain = 0;
        _animator.SetBool(IsSoulsHarvest, false);
    }
}