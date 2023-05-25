using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStateMachine : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("NeedMoveEnemyState")]
    [SerializeField] private EnemyState _startState;

    private Enemy _enemy;
    private Player _target;
    private EnemyState _currentState;

    public EnemyState CurrentState => _currentState;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _target = _enemy.Target;

        if (_currentState != null)
            _currentState.Exit();

        Reset(_startState);
    }

    private void OnDisable()
    {
        if (_target != null)
            Transit(_startState);
    }

    private void Update()
    {
        if (_currentState == null)
            return;

        var nextState = _currentState.GetNextState();

        if (nextState != null)
            Transit(nextState);
    }

    private void Reset(EnemyState startState)
    {
        _currentState = startState;

        if (_currentState != null)
            _currentState.Enter(_target);
    }

    private void Transit(EnemyState nextState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = nextState;
        _currentState.Enter(_target);
    }
}