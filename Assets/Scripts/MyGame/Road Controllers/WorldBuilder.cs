using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = System.Random;

public class WorldBuilder : SingletonBehaviour<WorldBuilder>
{
    private Transform _lastPlatform = null;
    private PoolType _lastRoadType;

    private bool _isObstacle;

    private bool _playerIsRocket = false;

    private Random _random;
    private void OnEnable()
    {
        RoadEnd.onRoadEnd += CreatePlatform;
        RoadEnd.onRoadEnd += OnRoadEnd;
        EventHub.bonusRocketPickedUp += OnBonusRocketPickedUp;
    }

    private void OnBonusRocketPickedUp()
    {
        StartCoroutine(BonusRocketSettings());
    }

    private IEnumerator BonusRocketSettings()
    {
        _playerIsRocket = true;

        yield return new WaitForSeconds(5);

        _playerIsRocket = false;
    }

    private void OnDestroy()
    {
        RoadEnd.onRoadEnd -= CreatePlatform;
        RoadEnd.onRoadEnd -= OnRoadEnd;
        EventHub.bonusRocketPickedUp -= OnBonusRocketPickedUp;

        _playerIsRocket = false;
        StopAllCoroutines();
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    public void Init(int seed)
    {
        _random = new Random(seed);
        
        CreateFreePlatform();
        CreateObstaclePlatform();
        CreateObstaclePlatform();
        for (int i = 0; i < 5; i++)
        {
            CreatePlatform(null);
        }
    }

    public void CreatePlatform(PoolItem nothing)
    {
        //var chance = _random.Next(0, 100);
        switch (_lastRoadType)
        {
            case PoolType.RoadSmall: CreateObstaclePlatform(/*chance > 50 ? _lastRoadType : */PoolType.RoadMiddle);
                break;
            case PoolType.RoadMiddle: CreateObstaclePlatform(/*chance > 50 ? _lastRoadType : */PoolType.RoadLong);
                break;
            case PoolType.RoadLong: CreateObstaclePlatform(/*chance > 50 ? _lastRoadType : */PoolType.RoadSmall);
                break;
        }
        Debug.LogError("create");
    }
    
    private PoolItem CreateBasePlatform(PoolType platformType)
    {
        float yOffset = _random.Next(-1, 1);
        float zOffset = _random.Next(1, 4);
        if (_playerIsRocket)
            zOffset = 0;
        Transform endPoint = (_lastPlatform == null) ? transform : _lastPlatform.GetComponent<RoadBlock>().endPoint;
        Vector3 pos = (_lastPlatform == null) ? transform.position : endPoint.position + new Vector3(0, yOffset, zOffset);

        PoolItem result = PoolManager.GetRandom(platformType);
        
        _lastPlatform = SetSpawnSettings(result, pos, endPoint);
        _lastRoadType = platformType;

        return result;
    }

    private void CreateFreePlatform()
    {
        var platform = CreateBasePlatform(PoolType.RoadSmall);

        platform.GetComponent<RoadBlock>().HideObjects();
        // if(generateCoins)
        //     platform.GetComponent<RoadBlock>().GenerateCoins(_random.Next());
        
        _isObstacle = false;
    }

    private void CreateObstaclePlatform(PoolType roadType = PoolType.RoadMiddle)
    {
        CreateBasePlatform(roadType);
        
        //_lastPlatform.GetComponent<RoadBlock>().GenerateObstacles(_random.Next());
        
        _isObstacle = true;
    }

    private Transform SetSpawnSettings(PoolItem result, Vector3 pos, Transform endPoint)
    {
        Transform resultTransform = result.gameObject.transform;
        
        resultTransform.SetParent(transform);
        resultTransform.position = pos;
        resultTransform.rotation = endPoint.rotation;
        
        result.gameObject.SetActive(true);

        return resultTransform;
    }

    private void OnRoadEnd(PoolItem poolItem)
    {
        StartCoroutine(ReturnToPool(poolItem));
    }

    private IEnumerator ReturnToPool(PoolItem poolItem)
    {
        yield return new WaitForSecondsRealtime(1);
        Debug.LogError("return");
        PoolManager.Return(poolItem);
    }
}
