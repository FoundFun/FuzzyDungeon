using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public abstract class State : MonoBehaviour
    {
        [SerializeField] private List<Transition> _transitions;

        protected Mouse TargetMouse { get; private set; }

        public void Enter(Mouse targetMouse)
        {
            if (enabled == false)
            {
                TargetMouse = targetMouse;
                enabled = true;

                foreach (var transition in _transitions)
                {
                    transition.enabled = true;
                    transition.Init(TargetMouse);
                }
            }
        }

        public State GetNextState()
        {
            foreach (var transition in _transitions)
            {
                if (transition.NeedTransit)
                    return transition.TargetState;
            }

            return null;
        }

        public void Exit()
        {
            if (enabled == true)
            {
                foreach (var transition in _transitions)
                    transition.enabled = false;

                enabled = false;
            }
        }
    }
}