using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotator : MonoBehaviour
{

    public float SawSpeed = 200f;
    
    void Update()
    {
        transform.Rotate(Vector3.down * SawSpeed * Time.deltaTime);
    }
}
