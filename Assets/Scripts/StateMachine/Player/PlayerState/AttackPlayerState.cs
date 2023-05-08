using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
public class AttackPlayerState : PlayerState
{
    [Header("Settings")]
    [SerializeField] private float _dashSpeed;

    private const float MaxSpeed = 100;
    private const float DashTime = 0.05f;
    private const float Delay = 0.05f;

    private readonly int IsAttackHashAnimation = Animator.StringToHash("IsAttack");

    private Player _player;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Coroutine _coroutine;
    private float _currentTimeAttack;

    private void OnValidate()
    {
        _dashSpeed = Mathf.Clamp(_dashSpeed, 0, MaxSpeed);
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
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

        StopCoroutine(_coroutine);
    }

    private IEnumerator Attack()
    {
        _animator.SetBool(IsAttackHashAnimation, true);

        yield return new WaitForSeconds(Delay);

        _player.Attack();

        _currentTimeAttack = 0;

        Vector2 direction = (TargetMouse.transform.position - transform.position).normalized;

        while (_currentTimeAttack < DashTime)
        {
            _currentTimeAttack += Time.deltaTime;
            _rigidbody2D.velocity = new Vector2(direction.x, direction.y) * _dashSpeed;

            yield return null;
        }

        _rigidbody2D.velocity = Vector2.zero;
        _animator.SetBool(IsAttackHashAnimation, false);
        _player.StopAttack();
    }
}