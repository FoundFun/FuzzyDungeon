using UnityEngine;

public abstract class PlayerTransition : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerState _targetState;

    public PlayerState TargetState => _targetState;

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