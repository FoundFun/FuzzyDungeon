using System.Runtime.InteropServices;
using UnityEngine;

public class Yandex : MonoBehaviour
{
    [SerializeField] private AudioSource _gameAudio;
    [SerializeField] private AudioSource _menuAudio;

    [DllImport("__Internal")]
    private static extern void ShowAds();

    public void OnShowAds()
    {
        OnStop();
        ShowAds();
    }


    public void OnPlay()
    {
        _gameAudio.mute = false;
        _menuAudio.mute = false;
    }

    private void OnStop()
    {
        _gameAudio.mute = true;
        _menuAudio.mute = true;
    }
}