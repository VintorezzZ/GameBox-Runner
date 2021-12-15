using System;
using MyGame.Other;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour, IPoolObservable, IDamageable
{
    public event Action<Obstacle> onReturnToPool;
    [SerializeField] private CoinsSetup[] coinsSetups;

    private PoolItem _poolItem;
    
    public void Init()
    {
        foreach (var coinsSetup in coinsSetups)
        {
            coinsSetup.Init();
        }
    }

    public void DeActivateCoins()
    {
        if(coinsSetups.Length == 0)
            return;
        
        foreach (var setup in coinsSetups)
        {
            setup.DeActivate();
        }
    }
    
    public void ActivateCoins()
    {
        if(coinsSetups.Length == 0)
            return;
        
        var setup = Random.Range(0, coinsSetups.Length);
        coinsSetups[setup].Activate();
    }

    
    public void OnTakeFromPool()
    {
        
    }
    public void OnReturnToPool()
    {
        onReturnToPool?.Invoke(this);
    }
    public void TakeDamage()
    {
        
        PoolManager.Return(this._poolItem);
    }
}

