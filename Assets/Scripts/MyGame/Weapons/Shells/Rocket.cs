using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, IPoolObservable
{
    private PoolItem _poolItem;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform targetRotation;
    
    public void Init(PoolItem parentPoolItem)
    {
        if(!_poolItem)
            _poolItem = parentPoolItem;
    }

    private void Update()
    {
        transform.RotateAround(targetRotation.position, transform.forward, 520 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        Collider[] colliders = Physics.OverlapSphere(other.transform.position, 5);

        foreach (var coll in colliders)
        {
            if (coll.gameObject.TryGetComponent(out IDamageable iDamageable))
            {
                SetExplosionFXSettings(PoolManager.Get(PoolType.ExplosionsFX).transform, coll.transform);
                iDamageable.TakeDamage();
            }
        }
        
        SetExplosionFXSettings(PoolManager.Get(PoolType.ExplosionsFX).transform, other.transform);
        PoolManager.Return(_poolItem);
    }

    private void SetExplosionFXSettings(Transform explosion, Transform collision)
    {
        explosion.position = collision.position;
        explosion.gameObject.SetActive(true);
        SoundManager.Instance.PlayBoom();
    }


    public void OnReturnToPool()
    {
        trailRenderer.enabled = false;
        trailRenderer.emitting = false;
    }

    public void OnTakeFromPool()
    {
        trailRenderer.enabled = true;
        trailRenderer.emitting = true;
    }
}
