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

    private readonly int IsSoulsHarvest = Animator.StringToHash("IsSoulsHarvest");

    private List<Enemy> _enemies = new List<Enemy>();
    private List<Explosion> _explosions = new List<Explosion>();

    private Demon _demon;
    private Animator _animator;
    private Coroutine _coroutine;
    private AudioSource _explosionAudio;

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

    public void Init(Explosion explosion, List<Enemy> enemies, CinemachineVirtualCamera virtualCamera)
    {
        Explosion template = Instantiate(explosion);
        template.Init(virtualCamera);
        template.gameObject.SetActive(false);

        _enemies = enemies;
        _explosions.Add(template);
    }

    private IEnumerator Attack()
    {
        int index = Random.Range(0, _explosions.Count);

        _animator.SetBool(IsSoulsHarvest, true);
        _explosions[index].OnBlowUp(_demon);
        _explosionAudio.Play();

        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i].gameObject.activeSelf == true)
            {
                _enemies[i].OnDie();
            }

            yield return null;
        }

        _animator.SetBool(IsSoulsHarvest, false);
    }
}