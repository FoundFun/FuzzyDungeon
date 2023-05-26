using System.Runtime.InteropServices;
using UnityEngine;

public class Yandex : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAds();

    public void OnShowAds()
    {
        //ShowAds();
    }
}