using System.Collections;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Coroutine prevCoroutine;
    
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void CreateShotLine(Vector3 startPos, Vector3 endPos, Color color)
    {
        if(prevCoroutine != null)
            StopCoroutine(prevCoroutine);
        
        SetShotLineSettings(startPos, endPos, color);
        
        prevCoroutine = StartCoroutine(DisableShotLine(0.3f));   // Why It is Null ?? 
    }
    
    private void SetShotLineSettings(Vector3 startPos, Vector3 endPos, Color color)
    {
        Color startColor = color;
        startColor.a = 0;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = color;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        lineRenderer.enabled = true;
    }
    
    private IEnumerator DisableShotLine(float time)
    {
        yield return new WaitForSeconds(time);
        lineRenderer.enabled = false;
    }
}
