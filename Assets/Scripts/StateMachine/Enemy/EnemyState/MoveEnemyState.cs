using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MoveEnemyState : EnemyState
{
    [Header("Settings")]
    [SerializeField] private float _speed;

    private int _isRunHashAnimation = Animator.StringToHash("IsRun");

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _targetPosition;

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

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_targetPosition);
    }

    private void LateUpdate()
    {
        _targetPosition = Vector2.MoveTowards(transform.position, Target.transform.position, _speed * Time.deltaTime);
    }
}