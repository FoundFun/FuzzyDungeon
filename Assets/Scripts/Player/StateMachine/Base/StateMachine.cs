using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(Player))]
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private State _firstState;

        private Mouse _targetMouse;
        private State _currentState;

        public State CurrentState => _currentState;

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

        private void Reset(State startState)
        {
            _currentState = startState;

            if (_currentState != null)
                _currentState.Enter(_targetMouse);
        }

        private void Transit(State nextState)
        {
            if (_currentState != null)
                _currentState.Exit();

            _currentState = nextState;
            _currentState.Enter(_targetMouse);
        }
    }
}