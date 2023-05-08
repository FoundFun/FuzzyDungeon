using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Experience))]
public abstract class Player : MonoBehaviour
{
    [Header("Settings")]
    [Range(0, MaxHealth)]
    [SerializeField] private int _health;

    private const int MaxHealth = 4;
    private const float DelayTakeDamage = 2;

    private Vector3 _startPosition = Vector3.zero;

    private Experience _experience;
    private Coroutine _coroutine;
    private Mouse _targetMouse;
    private int _currentHealth;

    public event UnityAction<Player> Died;
    public event UnityAction<int> HealthChanged;
    public event UnityAction GameOver;

    public Mouse TargetMouse => _targetMouse;

    public bool AttackState { get; private set; }

    private void OnValidate()
    {
        _health = Mathf.Clamp(_health, 0, MaxHealth);
    }

    private void Awake()
    {
        _experience = GetComponent<Experience>();
        Reset();
    }

    public void SetLevel(int level)
    {
        _experience.GetLevel(level);
    }

    public void Init(Mouse target)
    {
        _targetMouse = target;
    }

    public void Reset()
    {
        _experience.Reset();
        transform.position = _startPosition;
        _currentHealth = _health;
        HealthChanged?.Invoke(_currentHealth);
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
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                GameOver?.Invoke();
                Died?.Invoke(this);
            }
            else
            {
                _coroutine = StartCoroutine(EnableInvulnerability());
            }
        }
    }

    public void AddExperience(int reward)
    {
        _experience.Add(reward);
    }

    private IEnumerator EnableInvulnerability()
    {
        yield return new WaitForSeconds(DelayTakeDamage);

        StopCoroutine(_coroutine);
        _coroutine = null;
    }
}