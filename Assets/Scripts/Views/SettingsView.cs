using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class SettingsView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button enterButton;
    
    public InputField playerNameInput;
        
    public override void Initialize()
    {
        backButton.onClick.AddListener(() =>
        {
            // Hide();
            // ViewManager.Show<MainView>();
            ViewManager.ShowLast();
        });
        enterButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("playername", playerNameInput.text);
        });
    }

    public override void Show()
    {
        base.Show();
        playerNameInput.text = PlayerPrefs.GetString("playername");
    }
}
