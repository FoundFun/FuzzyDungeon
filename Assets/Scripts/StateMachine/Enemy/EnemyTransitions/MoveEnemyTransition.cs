using UnityEngine;

public class MoveEnemyTransition : EnemyTransition
{
    [Header("Settings")]
    [SerializeField] private float AttackTime;

    private float _elapsedTime;

    private void Update()
    {
        if (_elapsedTime > AttackTime)
        {
            _elapsedTime = 0;
            NeedTransit = true;
        }

        _elapsedTime += Time.deltaTime;
    }
}
