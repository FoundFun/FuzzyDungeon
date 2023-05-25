using UnityEngine;

namespace Players
{
    public abstract class Transition : MonoBehaviour
    {
        [SerializeField] private State _targetState;

        public State TargetState => _targetState;

        public bool NeedTransit { get; protected set; }

        protected Mouse TargetMouse { get; private set; }

        public void Init(Mouse targetMouse)
        {
            TargetMouse = targetMouse;
        }

        private void OnEnable()
        {
            NeedTransit = false;
        }
    }
}