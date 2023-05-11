using System.Collections.Generic;
using UnityEngine;

public class SpawnerSmoke : ObjectPool<Smoke>
{
    [SerializeField] private Smoke[] _smokes;
    [SerializeField] private GameObject _containerSmokes;

    private List<Smoke> _pool;

    private void Awake()
    {
        _pool = Initialize(_smokes, _containerSmokes);
    }

    public void OnSpawned(Enemy enemy)
    {
        int index = Random.Range(0, _pool.Count);

        if (TryGetObject(out Smoke smoke, index))
        {
            smoke.OnBlowUp(enemy);
        }
    }
}
