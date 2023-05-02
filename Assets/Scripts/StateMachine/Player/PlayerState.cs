using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<PlayerTransition> _transitions;

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

    public PlayerState GetNextState()
    {
        foreach (var transition in _transitions)
        {
            if (transition.NeedTransit)
            {
                return transition.TargetState;
            }
        }

        return null;
    }

    public void Exit()
    {
        if (enabled == true)
        {
            foreach (var transition in _transitions)
            {
                transition.enabled = false;
            }

            enabled = false;
        }
    }
}