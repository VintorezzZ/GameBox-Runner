using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelsHandler : MonoBehaviour
{
   public List<PlayerModel> playerModels;
   public int currentModelIndex;

   private void Awake()
   {
      foreach (PlayerModel playerModel in playerModels)
      {
         playerModel.gameObject.SetActive(false);
      }
      
      if(PlayerPrefs.HasKey("playermodel"))
      {
         try
         {
            playerModels[PlayerPrefs.GetInt("playermodel")].gameObject.SetActive(true);
         }
         catch (Exception e)
         {
            Debug.LogError("No saved model in list. Set default model");
            playerModels[0].gameObject.SetActive(true);
         }
         
         currentModelIndex = PlayerPrefs.GetInt("playermodel");
      }
      else
      {
         playerModels[0].gameObject.SetActive(true);
         currentModelIndex = 0;
      }
   }

   public void ActivateModel(int modelIndex)
   {
      if(currentModelIndex == modelIndex || modelIndex == -1 || modelIndex >= playerModels.Count)
         return;
      
      DeActivateCurrentModel();
      playerModels[modelIndex].gameObject.SetActive(true);
      currentModelIndex = modelIndex;
      PlayerPrefs.SetInt("playermodel", currentModelIndex);
   }

   private void DeActivateCurrentModel()
   {
      playerModels[currentModelIndex].gameObject.SetActive(false);
   }
}


