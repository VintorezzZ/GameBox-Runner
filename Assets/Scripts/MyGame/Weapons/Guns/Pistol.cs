using UnityEngine;

public class Pistol : BaseWeapon
{
    private LineCreator _lineCreator;
    public override void Init(Transform rayCastPoint)
    {
        base.Init(rayCastPoint);
        _lineCreator = GetComponent<LineCreator>();
    }

    public override void Shoot()
    {
        base.Shoot();
        ProcessShoot();
    }

    private void ProcessShoot()
    {
        if (hit.collider)
        {
            if (hit.transform.parent.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage();
                
                Transform impact = PoolManager.Get(PoolType.Decals).transform;
                SetImpactSettings(impact);
            }

            _lineCreator.CreateShotLine(transform.position, hit.point, Color.red);
        }
        else
        {
            _lineCreator.CreateShotLine(transform.position, rayCastPoint.position + rayCastPoint.forward * RAY_DISTANCE, Color.yellow);
        }
    }

    private void SetImpactSettings(Transform impact)
    {
        impact.position = hit.point;
        impact.rotation = Quaternion.LookRotation(hit.normal);
        impact.gameObject.SetActive(true);
    }
}
