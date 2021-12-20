using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class SettingsView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private Toggle postProcessingToggle;
    
    
    public InputField playerNameInput;
        
    public override void Initialize()
    {
        base.Initialize();
        
        backButton.onClick.AddListener(() =>
        {
            ViewManager.ShowLast();
        });
        enterButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("playername", playerNameInput.text);
        });
        postProcessingToggle.onValueChanged.AddListener((value) =>
        {
            GameManager.Instance.postProcessVolume.enabled = value;
        });
    }

    public override void Show()
    {
        base.Show();
        playerNameInput.text = PlayerPrefs.GetString("playername");
    }
}
