using UnityEngine;

public class ExitBallTransition : PlayerTransition
{
    private const float AttackTime = 7f;

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