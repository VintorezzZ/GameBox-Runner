using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class CustomizationView : View
{
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [SerializeField] private Button closeButton;

    private PlayerModelsHandler _playerModelsHandler;
    private Player _player => RoomController.Instance.localPlayer;

    public override void Initialize()
    {
          leftArrow.onClick.AddListener(SelectPreviousPlayerModel);
          rightArrow.onClick.AddListener(SelectNextPlayerModel);
          closeButton.onClick.AddListener(() =>
          {
              ViewManager.Show<MainMenuView>();
          });
    }

    public override void Show()
    {
        base.Show();
        
        if(!_playerModelsHandler)
            _playerModelsHandler = _player.GetComponent<PlayerModelsHandler>();
    }

    private void SelectPreviousPlayerModel()
    {
        _playerModelsHandler.ActivateModel(_playerModelsHandler.currentModelIndex - 1);
    }
    
    private void SelectNextPlayerModel()
    {
        _playerModelsHandler.ActivateModel(_playerModelsHandler.currentModelIndex + 1);
    }
}
