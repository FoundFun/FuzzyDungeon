using UnityEngine;
using Players;

namespace Enemies
{
    public abstract class Transition : MonoBehaviour
    {
        [SerializeField] private State _targetState;

        public State TargetState => _targetState;

        public bool NeedTransit { get; protected set; }

        protected Player Target { get; private set; }

        private void OnEnable()
        {
            NeedTransit = false;
        }

        public void Init(Player target)
        {
            Target = target;
        }
    }
}