using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Demon))]
[RequireComponent(typeof(Animator))]
public class AirAttackPlayerState : PlayerState
{
    private const float Delay = 1.3f;

    private readonly int HashAirAnimation = Animator.StringToHash("IsAirAttack");

    private Demon _demon;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Coroutine _coroutine;

    private List<Explosion> _explosions = new List<Explosion>();

    private void Awake()
    {
        _demon = GetComponent<Demon>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
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

    public void Init(Explosion explosion, CinemachineVirtualCamera virtualCamera)
    {
        Explosion template = Instantiate(explosion);
        template.Init(virtualCamera);
        template.gameObject.SetActive(false);

        _explosions.Add(template);
    }

    private IEnumerator Attack()
    {
        float explosionDelay = 0.05f;

        _animator.SetBool(HashAirAnimation, true);

        Time.timeScale = 0.5f;

        yield return new WaitForSeconds(Delay);

        _animator.SetBool(HashAirAnimation, false);
        _rigidbody2D.MovePosition(TargetMouse.transform.position);

        Time.timeScale = 1;

        yield return new WaitForSeconds(explosionDelay);

        int index = Random.Range(0, _explosions.Count);
        _explosions[index].OnBlowUp(_demon);
    }
}