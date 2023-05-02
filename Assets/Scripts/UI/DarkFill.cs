using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DarkFill : MonoBehaviour
{
    [SerializeField] private Image _lock;

    public void Unlock()
    {
        _lock.gameObject.SetActive(false);
    }
}