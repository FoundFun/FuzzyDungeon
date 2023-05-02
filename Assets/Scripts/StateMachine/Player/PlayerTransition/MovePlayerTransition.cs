using UnityEngine;

public class MovePlayerTransition : PlayerTransition
{
    private const float AttackTime = 0.3f;

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