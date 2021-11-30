using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected RaycastHit hit;
    protected const float RAY_DISTANCE = 30f;
    protected Transform rayCastPoint;
    protected Vector3 targetDirection;
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected AudioSource audioSource;
    public virtual void Init(Transform rayCastPoint)
    {
        this.rayCastPoint = rayCastPoint;
    }
    public virtual void Shoot()
    {
        if (Physics.Raycast(rayCastPoint.position,rayCastPoint.forward , out hit, RAY_DISTANCE))
        {
            //Debug.DrawLine(WeaponManager.RayCastPoint.position, hit.point, Color.black, 2f);
            targetDirection = -(hit.point - transform.position).normalized;
        }
        else
        {
            //Debug.DrawRay(WeaponManager._rayCastPoint.position, WeaponManager._rayCastPoint.forward * 10f, Color.magenta, 20f);
            targetDirection = -rayCastPoint.forward;
        }
        
        //Debug.DrawRay(WeaponManager._rayCastPoint.position, WeaponManager._rayCastPoint.forward * 10f, Color.magenta, 20f);
        SoundManager.Instance.PlayFire(audioSource, shootSound);
    }
}
