using UnityEngine;

public class ExitAirTransition : PlayerTransition
{
    private const float AttackTime = 3f;

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