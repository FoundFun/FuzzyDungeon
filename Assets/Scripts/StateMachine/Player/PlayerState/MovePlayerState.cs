using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MovePlayerState : PlayerState
{
    [Header("Settings")]
    [SerializeField] private float _maxSpeed;

    private readonly int IsRunHashAnimation = Animator.StringToHash("IsRun");

    private float _smoothTime = 0.05f;

    private Animator _animator;
    private Vector3 _direction;
    private Vector2 _currentVelocity;

    private void OnValidate()
    {
        _maxSpeed = Mathf.Clamp(_maxSpeed, 0, float.MaxValue);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        _animator.SetBool(IsRunHashAnimation, false);
    }

    private void LateUpdate()
    {
        _direction = Vector2.SmoothDamp(transform.position, TargetMouse.transform.position,
            ref _currentVelocity, _smoothTime, _maxSpeed, Time.deltaTime);

        if (transform.position != _direction)
        {
            _animator.SetBool(IsRunHashAnimation, true);
            Move(_direction);
        }
        else
        {
            _animator.SetBool(IsRunHashAnimation, false);
        }
    }

    private void Move(Vector2 direction)
    {
        transform.position = direction;
    }
}