using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Puck : MonoBehaviour
{
    private AudioSource _swishAudio;

    private void Awake()
    {
        _swishAudio = GetComponent<AudioSource>();
    }

    public void HitWall()
    {
        if (gameObject.activeSelf == true)
        {
            _swishAudio.Play();
        }
    }
}