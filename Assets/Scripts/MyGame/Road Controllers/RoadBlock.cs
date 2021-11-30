using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RoadBlock : MonoBehaviour, IPoolObservable
{
    public Transform endPoint;
    public Transform[] obstaclePoints;
    public Transform[] coinsPoints;
    public Transform[] graphicsPoints;
    public List<Obstacle> pooledObstacles;
    public List<RoadGraphics> pooledGraphics;

    private PoolItem _poolItem;
    private RoadEnd[] _destroyers;
    private Transform _generatedObstacles;
    private Transform _generatedGraphics;

    private Random _random;
    private void Awake()
    {
        CreateObstaclesContainer();
        CreateGraphicsContainer();
    }

    private void Start()
    {
       _poolItem = GetComponent<PoolItem>();
       _destroyers = GetComponentsInChildren<RoadEnd>();

       foreach (RoadEnd destroyer in _destroyers)
       {
           destroyer.parentPoolItem = _poolItem;
       }
    }

    private void GenerateGraphics()
    {
        if (graphicsPoints.Length > 0)
        {
            for (int i = 0; i < graphicsPoints.Length; i++)
            {
                RoadGraphics roadGraphics = GetRoadGraphics();
                
                roadGraphics.transform.SetParent(_generatedGraphics);
                roadGraphics.transform.position = graphicsPoints[i].position;
                roadGraphics.transform.rotation = graphicsPoints[i].rotation;
                    
                roadGraphics.gameObject.SetActive(true);

                pooledGraphics.Add(roadGraphics);
            }
        }
    }

    private void CreateObstaclesContainer()
    {
        _generatedObstacles = new GameObject("GeneratedObstacles").transform;
        _generatedObstacles.SetParent(transform);
    }
    private void CreateGraphicsContainer()
    {
        _generatedGraphics = new GameObject("GeneratedGraphics").transform;
        _generatedGraphics.SetParent(transform);
    }

    public void GenerateObstacles(int next)
    {
        _random = new Random(next);
        
        if (obstaclePoints.Length > 0)
        {
            for (int i = 0; i < obstaclePoints.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Obstacle roadItem = GetRoadItem();

                    roadItem.onReturnToPool += RemoveObstacleFromList;

                    roadItem.transform.SetParent(_generatedObstacles);
                    roadItem.transform.position = obstaclePoints[i].position;
                    roadItem.transform.rotation = obstaclePoints[i].rotation;
                    
                    roadItem.gameObject.SetActive(true);

                    pooledObstacles.Add(roadItem);
                }
            }
        }
    }

    private Obstacle GetRoadItem()
    {
        Obstacle roadItem;
        
        if (_random.Next(0, 100) < 10)
            roadItem = PoolManager.Get(PoolType.Bonuses).GetComponent<Obstacle>();
        else
            roadItem = PoolManager.GetRandom(PoolType.Obstacles).GetComponent<Obstacle>();

        return roadItem;
    }
    
    private RoadGraphics GetRoadGraphics()
    {
        RoadGraphics roadItem = PoolManager.GetRandom(PoolType.RoadGraphics).GetComponent<RoadGraphics>();

        return roadItem;
    }

    
    public void OnReturnToPool()
    {
        ReturnObstaclesToPool();
        ReturnGraphicsToPool();
    }

    public void ReturnObstaclesToPool()
    {
        foreach (var obst in pooledObstacles)
        {
            obst.onReturnToPool -= RemoveObstacleFromList;
            obst.RemoveObstacle();
        }

        pooledObstacles.Clear();
    }
    public void ReturnGraphicsToPool()
    {
        foreach (var graph in pooledGraphics)
        {
            graph.RemoveGraphics();
        }

        pooledGraphics.Clear();
    }

    private void RemoveObstacleFromList(Obstacle obst)
    {
        obst.onReturnToPool -= RemoveObstacleFromList;
        pooledObstacles.Remove(obst);
    }

    public void OnTakeFromPool()
    {
        GenerateGraphics();
    }

    public void GenerateCoins(int next)
    {
        _random = new Random(next);
        
        if (coinsPoints.Length > 0)
        {
            foreach (var coin in coinsPoints)
            {
                coin.gameObject.SetActive(true);
            }
        }
    }
}
