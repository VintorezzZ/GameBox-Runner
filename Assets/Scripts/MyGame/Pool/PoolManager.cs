using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<PoolType, PoolContainer> _pools = new Dictionary<PoolType, PoolContainer>();

    void Awake()
    {
        Instance = this;


        // пройти по всем дочерним контейнерам, добав в дикт и вызыв у него  инит. там он делает что-то, вешает на себя пул итем.
        foreach (PoolContainer poolContainer in GetComponentsInChildren<PoolContainer>())
        {
            _pools.Add(poolContainer.poolType, poolContainer);
            poolContainer.Init();
        }
    }

    public static PoolItem Get(PoolType poolType)
    {
        if (!Instance._pools.ContainsKey(poolType))
        {
            Debug.LogError("Unknown pool name: " + poolType);
            return null;
        }

        return Instance._pools[poolType].TakeFromPool();
    }

    public static PoolItem GetRandom(PoolType poolType)
    {
        if (!Instance._pools.ContainsKey(poolType))
        {
            Debug.LogError("Unknown pool name: " + poolType);
            return null;
        }

        return Instance._pools[poolType].TakeRandomFromPool();
    }

    public static void Return(PoolItem item)
    {
        if (!Instance._pools.ContainsKey(item.PoolType))
        {
            Debug.LogError("Unknown pool name: " + item.PoolType);
            return;
        }

        Instance._pools[item.PoolType].ReturnToPool(item);
    }
}
