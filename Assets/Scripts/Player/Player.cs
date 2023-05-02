using UnityEngine;
using UnityEngine.Events;

public abstract class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _health;

    private const int StartLevel = 1;

    private int _targetLevel = 20;
    private Vector3 _startPosition = Vector3.zero;

    private Mouse _targetMouse;
    private int _currentHealth;
    private int _level;
    private int _targetExperience;
    private int _currentExperience;

    public event UnityAction<Player> Died;
    public event UnityAction<int> LevelChanged;
    public event UnityAction<int> HealthChanged;
    public event UnityAction GameOver;

    public Mouse TargetMouse => _targetMouse;

    public bool AttackState { get; private set; }

    private void Awake()
    {
        _level = StartLevel;
        _targetExperience = 1;
        LevelChanged?.Invoke(_level);
        _currentExperience = 0;
        Reset();
    }

    public void SetLevel(int level)
    {
        _level = level;
        LevelChanged?.Invoke(_level);
    }

    public void Init(Mouse target)
    {
        _targetMouse = target;
    }

    public void Reset()
    {
        _level = StartLevel;
        transform.position = _startPosition;
        _currentHealth = _health;
        HealthChanged?.Invoke(_currentHealth);
        LevelChanged?.Invoke(_level);
    }

    public void SetAttackState(bool state)
    {
        AttackState = state;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        HealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            GameOver?.Invoke();
            Died?.Invoke(this);
        }
    }

    public void AddExperience(int reward)
    {
        _currentExperience += reward;

        if (_currentExperience >= _targetExperience)
        {
            _level++;
            LevelChanged?.Invoke(_level);
            _currentExperience = 0;

            if (_level > _targetLevel)
            {
                _targetLevel *= 2;
                _targetExperience++;
            }
        }
    }
}