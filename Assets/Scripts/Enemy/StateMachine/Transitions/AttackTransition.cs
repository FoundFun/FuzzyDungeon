using UnityEngine;

namespace Enemies
{
    public class AttackTransition : Transition
    {
        [Range(MinTransitionRange, MaxTransitionRange)]
        [SerializeField] private float _transitionRange;
        [Range(MinRangetSpread, MaxRangetSpread)]
        [SerializeField] private float _rangetSpread;

        private const float AttackTime = 2;
        private const float MaxRangetSpread = 1f;
        private const float MinRangetSpread = 0f;
        private const float MaxTransitionRange = 5f;
        private const float MinTransitionRange = 1f;

        private float _delay;

        private void OnValidate()
        {
            _transitionRange = Mathf.Clamp(_transitionRange, MinTransitionRange, MaxTransitionRange);
            _rangetSpread = Mathf.Clamp(_rangetSpread, MinRangetSpread, MaxRangetSpread);
        }

        private void Start()
        {
            _transitionRange += Random.Range(-_rangetSpread, _rangetSpread);
            _delay = 0;
        }

        private void Update()
        {
            _delay += Time.deltaTime;

            if (Vector2.Distance(transform.position, Target.transform.position)
                <= _transitionRange && _delay > AttackTime)
                NeedTransit = true;
        }
    }
}