using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerModelsHandler : MonoBehaviour
{
   public List<PlayerModel> playerModels;
   public int currentModelIndex;
   [SerializeField] private PlayerModel rocketModel;
   
   private void Awake()
   {
      EventHub.bonusRocketPickedUp += ActivateRocketModel;
      
      foreach (PlayerModel playerModel in playerModels)
      {
         if(playerModel.modelId == 0)
            PlayerPrefs.SetInt($"player_model_{playerModel.modelId}_own_status", 1);
         
         playerModel.Init();
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
      EventHub.OnPlayerModelChanged(modelIndex);
   }

   private void DeActivateCurrentModel()
   {
      playerModels[currentModelIndex].gameObject.SetActive(false);
   }

   public void ActivateRocketModel()
   {
      StartCoroutine(ActivateRocket());
   }

   private IEnumerator ActivateRocket()
   {
      DeActivateCurrentModel();
      rocketModel.gameObject.SetActive(true);

      yield return new WaitForSeconds(5);
      
      rocketModel.gameObject.SetActive(false);
      playerModels[currentModelIndex].gameObject.SetActive(true);
   }

   private void OnDisable()
   {
      EventHub.bonusRocketPickedUp -= ActivateRocketModel;
   }
}


