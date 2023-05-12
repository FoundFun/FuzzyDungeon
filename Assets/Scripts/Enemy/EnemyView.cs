using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyView : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Enemy _enemy;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        _sprite.flipX = _enemy.Target.transform.position.x < transform.position.x;
    }
}