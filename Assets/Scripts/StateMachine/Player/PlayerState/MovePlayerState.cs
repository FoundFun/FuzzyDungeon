using UnityEngine;

public class MovePlayerState : PlayerState
{
    [Header("Settings")]
    [SerializeField] private float _speed;

    private readonly int IsRunHashAnimation = Animator.StringToHash("IsRun");

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _direction;

    private void OnValidate()
    {
        _speed = Mathf.Clamp(_speed, 0, float.MaxValue);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        _animator.SetBool(IsRunHashAnimation, false);
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position != _direction)
        {
            if (_animator.GetBool(IsRunHashAnimation) == false)
            {
                _animator.SetBool(IsRunHashAnimation, true);
            }

            Move(_direction);
        }
        else
        {
            if (_animator.GetBool(IsRunHashAnimation) == true)
            {
                _animator.SetBool(IsRunHashAnimation, false);
            }
        }
    }

    private void LateUpdate()
    {
        _direction = Vector3.MoveTowards(transform.position, TargetMouse.transform.position, _speed * Time.deltaTime);
    }

    private void Move(Vector2 direction)
    {
        _rigidbody2D.MovePosition(direction);
    }
}