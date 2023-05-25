using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Player))]
public class PlayerView : MonoBehaviour
{
    private Player _player;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _player = GetComponent<Player>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_player.AttackState == false)
            FlipSprite(_player.TargetMouse.transform.position);
    }

    private void FlipSprite(Vector3 targetMousePosition)
    {
        _sprite.flipX = targetMousePosition.x < transform.position.x
            && targetMousePosition.x != transform.position.x
            ? _sprite.flipX = true : _sprite.flipX = false;
    }
}