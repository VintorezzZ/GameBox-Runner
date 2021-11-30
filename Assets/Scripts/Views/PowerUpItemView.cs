using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MyGame.Other
{
    public class PowerUpItemView : MonoBehaviour
    {
        public PowerUpType type;
        private float _duration;
        [SerializeField] private Image image;
        [SerializeField] private Image background;

        private Timer _timer = new Timer();
        
        public void Init(PowerUpType type, float duration, Sprite image, Sprite background)
        {
            this.type = type;
            _duration = duration;
            this.image.sprite = image;
            this.background.sprite = background;
            
            Activate();
        }

        private void Update()
        {
            if(!_timer.IsStarted)
                return;
            
            background.fillAmount = Mathf.Clamp(1f - _timer.Time / _duration, 0f, 1f);
            
            if(_timer.Time >= _duration)
                DeActivate();
        }

        private void Activate()
        {
            _timer.Stop();
            _timer.Start();
            background.fillAmount = 1;
        }
        
        private void DeActivate()
        {
            _timer.Stop();
            background.fillAmount = 0;
            gameObject.SetActive(false);
        }
    }
}