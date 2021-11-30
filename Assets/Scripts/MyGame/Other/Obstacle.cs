using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObservable, IDamageable
{
    public event Action<Obstacle> onReturnToPool;

    private PoolItem _poolItem;

    private void Start()
    {
        _poolItem = GetComponent<PoolItem>();
    }

    public void RemoveObstacle()
    {
        PoolManager.Return(this._poolItem);
    }
    public void OnReturnToPool()
    {
        onReturnToPool?.Invoke(this);  
    }

    public void OnTakeFromPool()
    {
        
    }


    public void TakeDamage()
    {
        
        PoolManager.Return(this._poolItem);
    }
}

