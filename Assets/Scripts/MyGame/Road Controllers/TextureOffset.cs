using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffset : MonoBehaviour
{

    // Scroll main texture based on time

    public float scrollSpeed = 0.5f;
    Renderer rend;
    public float X;
    public float Y;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetTextureScale("_MainTex", new Vector2(X, Y));
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
