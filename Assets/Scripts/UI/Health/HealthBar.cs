using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Heart _heart;
    [SerializeField] private Transform[] _spawnPoint;

    private List<Heart> _hearts = new List<Heart>();

    public void OnHealthChanged(int value)
    {
        if (_hearts.Count < value)
        {
            int createHearts = value - _hearts.Count;

            for (int i = 0; i < createHearts; i++)
            {
                CreateHeart(_heart, i);
            }
        }
        else if (_hearts.Count > value && _hearts.Count != 0)
        {
            int deleteHearts = _hearts.Count - value;

            for (int i = 0; i < deleteHearts; i++)
            {
                DestroyHeart(_hearts[_hearts.Count - 1]);
            }
        }
    }

    private void DestroyHeart(Heart heart)
    {
        heart.Deactivate();
        _hearts.Remove(heart);
    }

    private void CreateHeart(Heart heart, int index)
    {
        Heart template = Instantiate(heart, _spawnPoint[index]);
        template.gameObject.SetActive(true);

        _hearts.Add(template);
    }
}