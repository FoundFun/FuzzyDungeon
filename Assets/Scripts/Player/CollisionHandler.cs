using UnityEngine;
using Enemies;

namespace Players
{
    [RequireComponent(typeof(Player))]
    public class CollisionHandler : MonoBehaviour
    {
        private Player _player;

        private void Start()
        {
            _player = GetComponent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Enemy enemy) && _player.AttackState == true)
            {
                enemy.OnDie();
                _player.AddExperience(enemy.Experience);
            }
        }
    }
}