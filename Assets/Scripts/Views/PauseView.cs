using Com.MyCompany.MyGame;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class PauseView : View
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button resumeButton;
        public override void Initialize()
        {
            exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
            resumeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.UnpauseGame();
                Hide();
            });
        }
    }
}
