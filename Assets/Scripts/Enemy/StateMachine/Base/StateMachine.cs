using UnityEngine;
using Players;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private State _startState;

        private Enemy _enemy;
        private Player _target;
        private State _currentState;

        public State CurrentState => _currentState;

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

        private void Reset(State startState)
        {
            _currentState = startState;

            if (_currentState != null)
                _currentState.Enter(_target);
        }

        private void Transit(State nextState)
        {
            if (_currentState != null)
                _currentState.Exit();

            _currentState = nextState;
            _currentState.Enter(_target);
        }
    }
}