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
        _swishAudio.Play();
    }
}