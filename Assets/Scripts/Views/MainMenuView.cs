using Com.MyCompany.MyGame;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class MainMenuView : View
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button customizationButton;
    [SerializeField] private Button audioSettingsButton;
        
    public override void Initialize()
    {
        exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        audioSettingsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            ViewManager.Show<AudioSettingsView>();
        });
        settingsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            ViewManager.Show<SettingsView>();
        });
        customizationButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            ViewManager.Show<CustomizationView>();
        });
        
        startButton.onClick.AddListener(() =>
        {
            RoomController.Instance.startTimer.Start();
            ViewManager.Show<InGameView>();
        });
    }
}
