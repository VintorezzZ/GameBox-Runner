using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGraphics : MonoBehaviour
{
    private PoolItem _poolItem;
    void Start()
    {
        _poolItem = GetComponent<PoolItem>();
    }

    public void RemoveGraphics()
    {
        PoolManager.Return(this._poolItem);
    }


}
