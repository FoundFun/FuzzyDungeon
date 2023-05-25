using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerStateMachine : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("NeedMovePlayerState")]
    [SerializeField] private PlayerState _firstState;

    private Mouse _targetMouse;
    private PlayerState _currentState;

    public PlayerState CurrentState => _currentState;

    private void Start()
    {
        _targetMouse = GetComponent<Player>().TargetMouse;
        Reset(_firstState);
    }

    private void Update()
    {
        if (_currentState == null)
            return;

        var nextState = _currentState.GetNextState();

        if (nextState != null)
            Transit(nextState);
    }

    private void Reset(PlayerState startState)
    {
        _currentState = startState;

        if (_currentState != null)
            _currentState.Enter(_targetMouse);
    }

    private void Transit(PlayerState nextState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = nextState;
        _currentState.Enter(_targetMouse);
    }
}