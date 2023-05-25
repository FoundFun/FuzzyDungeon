using UnityEngine;

namespace Players
{
    public class ExitSoulsHarvestTransition : Transition
    {
        [SerializeField] private float AttackTime = 2;

        private float _elapsedTime = 0;

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