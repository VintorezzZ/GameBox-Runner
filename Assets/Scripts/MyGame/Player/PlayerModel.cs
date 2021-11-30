using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private string modelName;
    
    void Awake()
    {
        if (modelName == "")
        {
            modelName = gameObject.name;
        }
    }

}
