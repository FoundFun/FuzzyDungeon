using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ShadowCaster2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _damage;
    [SerializeField] private int _experience;

    private Player _target;
    private Collider2D _collider2D;
    private ShadowCaster2D _shadowCaster2D;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _coroutine;
    private AudioSource _hitAudio;

    public int Damage => _damage;
    public int Experience => _experience;
    public Player Target => _target;

    public bool AttackState { get; private set; }

    public event UnityAction<Enemy> Died;

    private void Awake()
    {
        _hitAudio = GetComponent<AudioSource>();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shadowCaster2D = GetComponent<ShadowCaster2D>();
    }

    private void OnEnable()
    {
        _collider2D.enabled = true;
        _spriteRenderer.enabled = true;
        _shadowCaster2D.enabled = true;
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(Die());
        }

        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
        _shadowCaster2D.enabled = false;
    }

    public void Init(Player target)
    {
        _target = target;
    }

    public void Attack()
    {
        AttackState = true;
    }

    public void StopAttack()
    {
        AttackState = false;
    }

    public void OnDie()
    {
        Died?.Invoke(this);

        if (_coroutine != null)
        {
            StopCoroutine(Die());
        }

        _coroutine = StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        _hitAudio.Play();

        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
        _shadowCaster2D.enabled = false;

        yield return new WaitForSeconds(_hitAudio.clip.length);

        gameObject.SetActive(false);
    }
}