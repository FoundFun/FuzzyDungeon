using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
    private PlayerStateMachine _state;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _state = GetComponent<PlayerStateMachine>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FlipSprite(_state.MousePosition);
    }

    private void FlipSprite(Vector3 targetMousePosition)
    {
        if (targetMousePosition.x < transform.position.x && _sprite.flipX == false)
        {
            _sprite.flipX = true;
        }
        else if (targetMousePosition.x > transform.position.x && _sprite.flipX == true)
        {
            _sprite.flipX = false;
        }
    }
}