using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Animator))]
    public class MoveState : State
    {
        [SerializeField] private float _speed;

        private int _isRunHashAnimation = Animator.StringToHash("IsRun");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _animator.SetBool(_isRunHashAnimation, true);
        }

        private void OnDisable()
        {
            _animator.SetBool(_isRunHashAnimation, false);
        }

        private void LateUpdate()
        {
            transform.position = Vector2.MoveTowards(transform.position,
                Target.transform.position, _speed * Time.deltaTime);
        }
    }
}