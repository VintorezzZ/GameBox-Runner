using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Other;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : Bonus
{
    [SerializeField] private bool enableRotation;
    [SerializeField] private float rotationSpeed;
    
    private void OnEnable()
    {
        rotationSpeed = Random.Range(0.5f, 2);
    }

    private void Update()
    {
        if(!enableRotation)
            return;
        
        transform.Rotate(0f, rotationSpeed, 0f);
    }
}
