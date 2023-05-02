using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Enemy))]
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
        if (_enemy.Target.transform.position.x < transform.position.x)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }
    }
}