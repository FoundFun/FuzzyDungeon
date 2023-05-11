using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Demon))]
[RequireComponent(typeof(SpriteRenderer))]
public class BallAttackPlayerState : PlayerState
{
    [Header("Settings")]
    [SerializeField] private float _moveSpeed;

    private const float Delay = 2;
    private const float ShakingTime = 0.3f;
    private const float IntensityShaking = 0.4f;

    private Demon _demon;
    private Puck _puck;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _lastVelocity;
    private Coroutine _attackCoroutine;
    private Coroutine _shakingCoroutine;
    private CinemachineBasicMultiChannelPerlin _virtualCamera;

    private void Awake()
    {
        _demon = GetComponent<Demon>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }

        _demon.Attack();
        _attackCoroutine = StartCoroutine(Attack());
    }

    private void OnDisable()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }

        if (_shakingCoroutine != null)
        {
            StopCoroutine(_shakingCoroutine);
        }

        _demon.StopAttack();
        _rigidbody2D.velocity = Vector2.zero;
        _virtualCamera.m_AmplitudeGain = 0;
        _spriteRenderer.enabled = true;
        _puck.gameObject.SetActive(false);
    }

    private void Update()
    {
        _lastVelocity = _rigidbody2D.velocity;
        _puck.transform.position = _demon.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = _lastVelocity.magnitude;
        var direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
        _virtualCamera.m_AmplitudeGain = 0.5f;

        if (_shakingCoroutine != null)
        {
            StopCoroutine(_shakingCoroutine);
        }

        _shakingCoroutine = StartCoroutine(HitWall());
        _puck.HitWall();
        _rigidbody2D.velocity = direction * Mathf.Max(speed, 0);
    }

    public void Init(Puck puck, CinemachineVirtualCamera virtualCamera)
    {
        _puck = Instantiate(puck);
        _puck.gameObject.SetActive(false);

        _virtualCamera = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _virtualCamera.m_AmplitudeGain = 0;
    }

    private IEnumerator Attack()
    {
        _spriteRenderer.enabled = false;
        _puck.transform.position = _demon.transform.position;
        _puck.gameObject.SetActive(true);

        Time.timeScale = 0.5f;

        yield return new WaitForSeconds(Delay);

        Time.timeScale = 1;

        Vector2 direction = (TargetMouse.transform.position - transform.position).normalized;

        _rigidbody2D.AddForce(direction * _moveSpeed, ForceMode2D.Impulse);
    }

    private IEnumerator HitWall()
    {
        _virtualCamera.m_AmplitudeGain = IntensityShaking;

        yield return new WaitForSeconds(ShakingTime);

        _virtualCamera.m_AmplitudeGain = 0;
    }
}