using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using MyGame.Other;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Utils;
using Views;

namespace MyGame.Managers
{
    public class PickUpHandler : MonoBehaviour
    {
        [SerializeField] private GameObject awesomeSpritePrefab;
        [SerializeField] private List<Sprite> awesomeSprites;
        
        private global::Player _player;

        public void Init(global::Player player)
        {
            _player = player;
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckForBulletBonus(other);
            CheckForCoin(other);
        }

        private void CheckForBulletBonus(Collider other)
        {
            if (other.TryGetComponent(out PowerUp powerUp))
            {
                ViewManager.GetView<InGameView>().ActivatePowerUp(powerUp);
                _player.weaponManager.SwitchWeapon("RPG7", powerUp.duration);

                _player.Ammo++;

                if (_player.Ammo > 30) 
                    _player.Ammo = 30;
            
                SoundManager.Instance.PlayPickUp();
                
                PoolManager.Return(other.gameObject.GetComponent<PoolItem>());
            }
        }
        
        private void CheckForCoin(Collider other)
        {
            if (other.TryGetComponent(out Coin coin))
            {
                _player.Coins += GameSettings.Config.pickUpBonusCoins;
            
                SoundManager.Instance.PlayCoinPickUp();
                
                other.gameObject.SetActive(false);
                //PoolManager.Return(other.gameObject.GetComponent<PoolItem>());
            }
        }
    }
}