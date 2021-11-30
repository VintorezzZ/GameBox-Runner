using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour
{
    public bool isFree { get; private set; }
    public PoolType PoolType { get; private set; }

    private List<IPoolObservable> _observableComponents = new List<IPoolObservable>();
 
    public void Init(PoolType poolType)
    {
        isFree = true;
        PoolType = poolType;
        
        _observableComponents.AddRange(GetComponentsInChildren<IPoolObservable>());
    }

    public void TakeFromPool()
    {
        isFree = false;

        foreach (var observableComponent in _observableComponents)
        {
            observableComponent.OnTakeFromPool();
        }
    }

    public void ReturnToPool()
    {
        isFree = true;
        
        foreach (var observableComponent in _observableComponents)
        {
            observableComponent.OnReturnToPool();
        }
    }
}
