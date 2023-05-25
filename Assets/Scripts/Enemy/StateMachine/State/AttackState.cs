using System.Collections;
using UnityEngine;
using Players;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class AttackState : State
    {
        [Range(MinDashSpeed, MaxDashSpeed)]
        [SerializeField] private float _dashSpeed;
        [Range(MinDashLengh, MaxDashLengh)]
        [SerializeField] private float _dashLength;

        private const float AttackDelay = 0.5f;
        private const int MaxDashSpeed = 100;
        private const int MinDashSpeed = 10;
        private const int MaxDashLengh = 8;
        private const int MinDashLengh = 1;

        private readonly int _isAttackHashAnimation = Animator.StringToHash("IsAttack");

        private Enemy _enemy;
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private Coroutine _coroutine;

        private void OnValidate()
        {
            _dashSpeed = Mathf.Clamp(_dashSpeed, MinDashSpeed, MaxDashSpeed);
            _dashLength = Mathf.Clamp(_dashLength, MinDashLengh, MaxDashLengh);
        }

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Attack(Target));
        }

        private void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator Attack(Player target)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            Vector3 startPosition = transform.position;

            yield return new WaitForSeconds(AttackDelay);

            _animator.SetBool(_isAttackHashAnimation, true);

            _enemy.Attack();
            _rigidbody2D.velocity = new Vector2(direction.x, direction.y) * _dashSpeed;

            yield return new WaitWhile(() => Vector3.Distance(startPosition, transform.position) < _dashLength);

            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetBool(_isAttackHashAnimation, false);
            _enemy.StopAttack();
        }
    }
}