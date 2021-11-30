using System;
using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour, IPoolObservable
{
   private float speed = 50f;
   private PoolItem _poolItem;
   private Rocket _rocket;
   private Vector3 _targetDirection;
   public float PlayerVelocity { get; set; }

   public void Init(Vector3 targetDir)
   {
      if(!_poolItem)
         _poolItem = GetComponent<PoolItem>();

      if (!_rocket)
         _rocket = GetComponentInChildren<Rocket>();

      _targetDirection = targetDir;
      _rocket.Init(_poolItem);
   }

   private void OnEnable()
   {
      StartCoroutine(ReturnToPool(2));
   }

   private void Update()
   {
      transform.localPosition += -_targetDirection * speed * Time.deltaTime;
   }
   private IEnumerator ReturnToPool(float time)
   {
      yield return new WaitForSeconds(time);
      PoolManager.Return(_poolItem);
   }

   public void OnReturnToPool()
   {
      StopCoroutine(ReturnToPool(2));
   }

   public void OnTakeFromPool()
   {
      //StartCoroutine(ReturnToPool(2));
   }
}
