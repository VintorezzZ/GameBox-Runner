using System;
using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using MyGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views;

public class CustomizationView : View
{
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private TMP_Text selectButtonText;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private GameObject costObject;

    private PlayerModelsHandler _playerModelsHandler;
    private Player _player => RoomController.Instance.localPlayer;

    public override void Initialize()
    {
        base.Initialize();

        EventHub.playerModelChanged += OnPlayerModelChanged;
        
        leftArrow.onClick.AddListener(delegate { ChangePlayerModel(-1); });
        rightArrow.onClick.AddListener(delegate { ChangePlayerModel(1); });
        selectButton.onClick.AddListener(SelectModel);
        closeButton.onClick.AddListener(() =>
        { 
            ViewManager.Show<MainMenuView>();
        });
    }

    private void SelectModel()
    {
        if (_playerModelsHandler.playerModels[_playerModelsHandler.currentModelIndex].status == SellItemStatus.ForSale)
        {
            var money = PlayerPrefs.GetInt("Total_coins");
            var item = _playerModelsHandler.playerModels[_playerModelsHandler.currentModelIndex];
            var itemCost = item.cost;
        
            if (money >= itemCost)
            {
                PlayerPrefs.SetInt("Total_coins", money - itemCost);
                PlayerPrefs.SetInt("playermodel", _playerModelsHandler.currentModelIndex);
                item.status = SellItemStatus.Bought;
                PlayerPrefs.SetInt($"player_model_{item.modelId}_own_status", 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("playermodel", _playerModelsHandler.currentModelIndex);
        }
        
        OnPlayerModelChanged(_playerModelsHandler.currentModelIndex);
    }

    public override void Show()
    {
        base.Show();
       
        if(!_playerModelsHandler)
            _playerModelsHandler = _player.GetComponent<PlayerModelsHandler>();
        
        if(!_playerModelsHandler)
            return;
        
        ChangePlayerModel(_playerModelsHandler.currentModelIndex);
        OnPlayerModelChanged(_playerModelsHandler.currentModelIndex);
    }

    public override void Hide()
    {
        base.Hide();
        
        if(_playerModelsHandler)
            _playerModelsHandler.ActivateModel(PlayerPrefs.GetInt("playermodel"));
    }

    private void ChangePlayerModel(int side)
    {
        var item = _playerModelsHandler.currentModelIndex + side;

        if (item < 0 || item >= _playerModelsHandler.playerModels.Count)
            return;
        
        _playerModelsHandler.ActivateModel(item);
    }

    private void OnPlayerModelChanged(int item)
    {
        switch (_playerModelsHandler.playerModels[item].status)
        {
            case SellItemStatus.ForSale : 
                selectButtonText.text = "BUY";
                cost.text = _playerModelsHandler.playerModels[item].cost.ToString();
                break;
            case SellItemStatus.Bought :
                selectButtonText.text = PlayerPrefs.GetInt("playermodel") == _playerModelsHandler.currentModelIndex ? "SELECTED" : "SELECT";
                break;
        }
        
        selectButton.interactable = (PlayerPrefs.GetInt("Total_coins") >= _playerModelsHandler.playerModels[item].cost 
                                    && PlayerPrefs.GetInt("playermodel") != _playerModelsHandler.currentModelIndex) 
                                    || (_playerModelsHandler.playerModels[item].status == SellItemStatus.Bought 
                                    && PlayerPrefs.GetInt("playermodel") != _playerModelsHandler.currentModelIndex);
        
        costObject.SetActive(_playerModelsHandler.playerModels[item].status == SellItemStatus.ForSale);
    }

    private void OnDestroy()
    {
        EventHub.playerModelChanged -= OnPlayerModelChanged;
    }
}
