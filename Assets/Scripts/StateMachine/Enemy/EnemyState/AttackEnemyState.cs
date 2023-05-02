using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class AttackEnemyState : EnemyState
{
    [Header("Settings")]
    [SerializeField] private float _delay;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashLength;

    private const float Delay = 0.5f;

    private readonly int _isAttackHashAnimation = Animator.StringToHash("IsAttack");

    private Enemy _enemy;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Coroutine _coroutine;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(Attack(Target));
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    private IEnumerator Attack(Player target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        Vector3 startPosition = transform.position;

        yield return new WaitForSeconds(Delay);

        _animator.SetBool(_isAttackHashAnimation, true);

        while (Vector3.Distance(startPosition, transform.position) < _dashLength)
        {
            _enemy.SetAttackState(true);
            _rigidbody2D.velocity = new Vector2(direction.x, direction.y) * _dashSpeed;

            yield return null;
        }

        _rigidbody2D.velocity = Vector2.zero;
        _animator.SetBool(_isAttackHashAnimation, false);
        _enemy.SetAttackState(false);
    }
}