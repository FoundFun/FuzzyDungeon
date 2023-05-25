using UnityEngine;

namespace Players
{
    public class ExitAirTransition : Transition
    {
        [SerializeField] private float AttackTime = 2.6f;

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
}