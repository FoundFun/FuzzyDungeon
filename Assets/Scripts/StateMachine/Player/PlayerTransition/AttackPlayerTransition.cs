using UnityEngine;

public class AttackPlayerTransition : PlayerTransition
{
    private const float DelayAttack = 0.1f;

    private float _currentTime;

    private void Update()
    {
        if (_currentTime > DelayAttack && Input.GetMouseButton(0) && Time.timeScale != 0)
        {
            _currentTime = 0;
            NeedTransit = true;
        }

        _currentTime += Time.deltaTime;
    }
}