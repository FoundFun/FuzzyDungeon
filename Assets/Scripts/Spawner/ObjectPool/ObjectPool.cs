using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    private List<T> _poolObject;

    protected List<T> Initialize(T[] gameObjects, GameObject targetContainer)
    {
        _poolObject = new List<T>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            T templte = Instantiate(gameObjects[i], targetContainer.transform);
            templte.gameObject.SetActive(false);

            _poolObject.Add(templte);
        }

        return _poolObject;
    }

    protected bool TryGetObject(out T gameObject, int indexElement)
    {
        gameObject = _poolObject.ElementAtOrDefault(indexElement);

        if (gameObject.gameObject.activeSelf == true)
            gameObject = null;

        return gameObject != null;
    }
}