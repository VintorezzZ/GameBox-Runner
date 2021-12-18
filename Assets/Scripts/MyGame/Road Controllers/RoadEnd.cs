using UnityEngine;

public class RoadEnd : MonoBehaviour
{
    public delegate void OnRoadEnds(PoolItem poolItem);
    public static event OnRoadEnds onRoadEnd;

    public PoolItem parentPoolItem;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            onRoadEnd?.Invoke(parentPoolItem);
        }
    }
}
