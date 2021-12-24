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

        [SerializeField] private Text scoreValueText;
        [SerializeField] private GameObject bestScoreText;
        [SerializeField] private GameObject scoreText;
        
        public override void Initialize()
        {
            base.Initialize();
            
            restartButton.onClick.AddListener(() =>
            {
                ViewManager.Show<MainMenuView>();
                GameManager.Instance.RestartGame();
            });

            EventHub.gameOvered += UpdateBestScore;
        }

        private void UpdateBestScore()
        {
            scoreValueText.text = RoomController.Instance.localPlayer.score.ToString();

            if (RoomController.Instance.localPlayer.score > PlayerPrefs.GetInt("HighScore_distance"))
            {
                PlayerPrefs.SetInt("HighScore_distance", Mathf.RoundToInt(RoomController.Instance.localPlayer.score));
                bestScoreText.SetActive(true);
                scoreText.SetActive(false);
            }
            else
            {
                bestScoreText.SetActive(false);
                scoreText.SetActive(true);
            }
        }
    }
}
