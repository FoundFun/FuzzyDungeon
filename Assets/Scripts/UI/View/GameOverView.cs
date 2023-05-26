using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameOverView : View
{
    [SerializeField] private TMP_Text _currentBestLevel;
    [SerializeField] private TMP_Text _bestLevel;

    public event UnityAction PlayButtonClick;

    public void OpenScreen()
    {
        Open();
    }

    public void CloseScreen()
    {
        Close();
    }

    protected override void Open()
    {
        base.Open();
        _bestLevel.text = _currentBestLevel.text;
    }

    protected override void Close()
    {
        base.Close();
    }

    protected override void OnPlayGameButtonClick()
    {
        PlayButtonClick?.Invoke();
    }
}