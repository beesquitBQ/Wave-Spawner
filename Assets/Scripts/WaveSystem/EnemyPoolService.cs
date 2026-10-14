using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPoolService : MonoBehaviour
{
    private readonly Dictionary<EnemyBase, IObjectPool<EnemyBase>> _pools = new();

    public EnemyBase Get(EnemyBase prefab)
    {
        if (!_pools.TryGetValue(prefab, out var pool))
        {
            pool = CreatePool(prefab);
            _pools.Add(prefab, pool);
        }

        return pool.Get();
    }

    public void Release(EnemyBase enemy)
    {
        if (_pools.TryGetValue(enemy.PrefabOrigin, out var pool))
        {
            pool.Release(enemy);
        }
        else
        {
            Destroy(enemy.gameObject);
        }
    }

    private IObjectPool<EnemyBase> CreatePool(EnemyBase prefab)
    {
        return new ObjectPool<EnemyBase>(
            createFunc: () =>
            {
                EnemyBase instance = Instantiate(prefab, transform);
                instance.PrefabOrigin = prefab;
                return instance;
            },
            actionOnGet: instance => { },
            actionOnRelease: instance => instance.gameObject.SetActive(false),
            actionOnDestroy: instance => Destroy(instance.gameObject),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );
    }
}