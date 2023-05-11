using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Demon))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class SoulsHarvestState : PlayerState
{
    [SerializeField] private float _propagationSpeed;

    private const float Delay = 1;

    private readonly int IsSoulsHarvest = Animator.StringToHash("IsSoulsHarvest");

    private List<Explosion> _explosions = new List<Explosion>();

    private Demon _demon;
    private Animator _animator;
    private Coroutine _coroutine;
    private AudioSource _explosionAudio;

    private event UnityAction KillAllEnemies;

    private void Awake()
    {
        _demon = GetComponent<Demon>();
        _animator = GetComponent<Animator>();
        _explosionAudio = GetComponent<AudioSource>();
        _explosionAudio.volume = 0.5f;
    }

    private void OnEnable()
    {
        _demon.Attack();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Attack());
    }
    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _demon.StopAttack();
    }

    public void Init(Explosion explosion, UnityAction OnKillAllEnemies,
        CinemachineVirtualCamera virtualCamera)
    {
        Explosion template = Instantiate(explosion);
        template.Init(virtualCamera);
        template.gameObject.SetActive(false);

        KillAllEnemies = OnKillAllEnemies;
        _explosions.Add(template);
    }

    private IEnumerator Attack()
    {
        int index = Random.Range(0, _explosions.Count);

        KillAllEnemies?.Invoke();
        _animator.SetBool(IsSoulsHarvest, true);
        _explosions[index].OnBlowUp(_demon);
        _explosionAudio.Play();

        yield return new WaitForSeconds(Delay);

        _animator.SetBool(IsSoulsHarvest, false);
    }
}