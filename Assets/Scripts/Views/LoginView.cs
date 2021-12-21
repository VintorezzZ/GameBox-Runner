using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class LoginView : View
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button loginButton;
        
    public InputField playerNameInput;
        
    public override void Initialize()
    {
        base.Initialize();
        
        exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        loginButton.onClick.AddListener(() =>
        {
            LoadMainMenu();
            PlayerPrefs.SetString("playername", playerNameInput.text);
        });
            
        GameSettings.GetSettingsFromFile();
    }

    private void LoadMainMenu()
    {
        ViewManager.Show<LoadingScreenView>();
        GameManager.Instance.LoadGameScene();
    }

    public override void Show()
    {
        base.Show();
        playerNameInput.text = "Player " + Random.Range(1000, 10000);
        
        if(PlayerPrefs.HasKey("playername"))
        {
            LoadMainMenu();
        }
    }
}