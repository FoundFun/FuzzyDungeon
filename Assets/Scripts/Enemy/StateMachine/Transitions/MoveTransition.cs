using UnityEngine;

namespace Enemies
{
    public class MoveTransition : Transition
    {
        [SerializeField] private float _attackTime;

        private float _elapsedTime;

        private void Update()
        {
            if (_elapsedTime > _attackTime)
            {
                _elapsedTime = 0;
                NeedTransit = true;
            }

            _elapsedTime += Time.deltaTime;
        }
    }
}