using UnityEngine;

public class Mouse : MonoBehaviour
{
    private const float _clampPositionX = 13.5f;
    private const float _clampPosotionY = 8;

    private Vector3 _mousePosition;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.position = new Vector2(Mathf.Clamp(_mousePosition.x, -_clampPositionX, _clampPositionX),
                                Mathf.Clamp(_mousePosition.y, -_clampPosotionY, _clampPosotionY));
    }
}