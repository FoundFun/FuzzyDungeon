using UnityEngine;

public abstract class EnemyTransition : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyState _targetState;

    public EnemyState TargetState => _targetState;

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