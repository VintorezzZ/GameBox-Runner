using System;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using MyGame.Other;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Views
{
    public class InGameView : View
    {
        [SerializeField] private PowerUpItemView[] powerUpsList;
        [SerializeField] private Image[] hearts;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text bulletsText;
        [SerializeField] private TMP_Text coinsText;
        public Text timerText;
        private Player _player => RoomController.Instance.localPlayer;
        private Dictionary<PowerUpType, PowerUpItemView> powerUps = new Dictionary<PowerUpType, PowerUpItemView>();

        public override void Initialize()
        {
            base.Initialize();
            
            EventHub.bulletsChanged += UpdateBullets;
            EventHub.coinsChanged += UpdateCoins;

            foreach (var item in powerUpsList)
            {
                if(powerUps.ContainsKey(item.type))
                    continue;
                
                powerUps.Add(item.type, item);
            }
            //
            // foreach (var heart in hearts)
            // {
            //     heart.gameObject.SetActive(false);
            // }
        }

        public override void Show()
        {
            base.Show();
            Clear();
            SoundManager.Instance.PlayInGameMusic();
        }

        private void Update()
        {
            if(!RoomController.Instance.isGameStarted)
                return;
            
            UpdateScore(_player.score);
        }

        private void Clear()
        {
            scoreText.text = "0";
            coinsText.text = "0";

            foreach (var pair in powerUps)
            {
                pair.Value.gameObject.SetActive(false);
            }
        }

        private void UpdateScore(float score)
        {
            var scoreToInt = Mathf.RoundToInt(score);
            scoreText.text = string.Format("{0}", scoreToInt);
        }

        public void AddHealth(int hp)
        {
            //hearts[hp].gameObject.SetActive(true);
        }
        
        public void RemoveHealth(int hp)
        {
            //hearts[hp].gameObject.SetActive(false);
        }

        private void UpdateBullets(int bullets)
        {
            //bulletsText.text = bullets + "/30"; 
        }

        private void UpdateCoins(int coins)
        {
            coinsText.text = string.Format("{0}", coins);
        }

        private void OnDestroy()
        {
            EventHub.scoreChanged -= UpdateScore;
            EventHub.bulletsChanged -= UpdateBullets;
        }

        public void ActivatePowerUp(PowerUp powerUp)
        {
            var item = powerUps[powerUp.type];
            
            item.gameObject.SetActive(true);
            item.Init(powerUp.type, powerUp.duration, powerUp.image, powerUp.background);
        }
    }
}
