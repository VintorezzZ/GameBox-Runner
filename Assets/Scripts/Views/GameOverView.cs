using Com.MyCompany.MyGame;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Views
{
    public class GameOverView : View
    {
        [SerializeField] private Button restartButton;

        [SerializeField] private Text bestScoreText;
        
        public override void Initialize()
        {
            restartButton.onClick.AddListener(() =>
            {
                ViewManager.Show<MainMenuView>();
                GameManager.Instance.RestartGame();
            });

            EventHub.gameOvered += UpdateBestScore;
        }

        private void UpdateBestScore()
        {
            bestScoreText.text = PlayerPrefs.GetFloat("bestscore").ToString();
        }
    }
}
