using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerStateMachine))]
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
        _sprite.flipX = targetMousePosition.x < transform.position.x;
    }
}