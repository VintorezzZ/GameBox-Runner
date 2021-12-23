using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private string modelName;
    public int modelId;
    public SellItemStatus status;
    public int cost;

    public void Init()
    {
        if (modelName == "")
        {
            modelName = gameObject.name;
        }
        
        status = (SellItemStatus)PlayerPrefs.GetInt($"player_model_{modelId}_own_status");
    }
}
