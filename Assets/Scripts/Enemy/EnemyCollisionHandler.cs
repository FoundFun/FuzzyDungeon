using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyCollisionHandler : MonoBehaviour
{
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player) && _enemy.AttackState == true)
        {
            if (player.AttackState == false)
                player.TakeDamage(_enemy.Damage);
        }
    }
}