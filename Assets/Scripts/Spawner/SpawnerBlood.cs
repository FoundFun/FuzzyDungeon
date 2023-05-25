using System.Collections.Generic;
using UnityEngine;

public class SpawnerBlood : ObjectPool<Blood>
{
    [SerializeField] private Blood[] _blood;
    [SerializeField] private GameObject _containerBloods;

    private List<Blood> _pool;

    private void Awake()
    {
        _pool = Initialize(_blood, _containerBloods);
    }

    public void OnDied(Enemy enemy)
    {
        int index = Random.Range(0, _pool.Count);

        if (TryGetObject(out Blood blood, index))
            blood.Play(enemy);
    }
}