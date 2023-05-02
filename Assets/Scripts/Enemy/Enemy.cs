using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _damage;
    [SerializeField] private int _experience;

    private Player _target;

    public event UnityAction<Enemy> Died;

    public int Damage => _damage;
    public int Experience => _experience;
    public Player Target => _target;

    public bool AttackState { get; private set; }

    public void Init(Player target)
    {
        _target = target;
    }

    public void SetAttackState(bool state)
    {
        AttackState = state;
    }

    public void Die()
    {
        Died?.Invoke(this);
        gameObject.SetActive(false);
    }
}