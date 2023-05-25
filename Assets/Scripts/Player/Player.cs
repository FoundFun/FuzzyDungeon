using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Players
{
    [RequireComponent(typeof(Experience))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Player : MonoBehaviour
    {
        [Range(0, MaxHealth)]
        [SerializeField] private int _health;

        private const int MaxHealth = 4;
        private const float DelayTakeDamage = 1.3f;

        private readonly int HitAnimation = Animator.StringToHash("IsHit");

        private Vector3 _startPosition = Vector3.zero;

        private Experience _experience;
        private Animator _animator;
        private Mouse _targetMouse;
        private AudioSource _audioSource;
        private Coroutine _coroutine;
        private int _currentHealth;

        public Mouse TargetMouse => _targetMouse;

        public bool AttackState { get; private set; }

        public event UnityAction<Player> Died;
        public event UnityAction<int> HealthChanged;

        private void OnValidate()
        {
            _health = Mathf.Clamp(_health, 0, MaxHealth);
        }

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = 1;
            Reset();
        }

        private void OnEnable()
        {
            HealthChanged?.Invoke(_currentHealth);
        }

        private void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        public void Reset()
        {
            _experience.Reset();
            transform.position = _startPosition;
            _currentHealth = _health;
            HealthChanged?.Invoke(_currentHealth);
        }

        public void SetLevel(int level)
        {
            _experience.GetLevel(level);
        }

        public void Init(Mouse target)
        {
            _targetMouse = target;
        }

        public void Attack()
        {
            AttackState = true;
        }

        public void StopAttack()
        {
            AttackState = false;
        }

        public void TakeDamage(int damage)
        {
            if (_coroutine == null)
            {
                _audioSource.Play();
                _currentHealth -= damage;
                HealthChanged?.Invoke(_currentHealth);
                _coroutine = StartCoroutine(EnableInvulnerability());

                if (_currentHealth <= 0)
                    Died?.Invoke(this);
            }
        }

        public void AddExperience(int reward)
        {
            _experience.Add(reward);
        }

        private IEnumerator EnableInvulnerability()
        {
            _animator.SetBool(HitAnimation, true);

            yield return new WaitForSeconds(DelayTakeDamage);

            _animator.SetBool(HitAnimation, false);
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}