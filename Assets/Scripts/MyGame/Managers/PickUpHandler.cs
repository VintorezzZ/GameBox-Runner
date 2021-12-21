using System.Collections.Generic;
using MyGame.Other;
using UnityEngine;
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
            if(_player.moveController.isRocketMovement)
                return;
            
            CheckForBulletBonus(other);
            CheckForBonus(other);
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
        
        private void CheckForBonus(Collider other)
        {
            if (!other.TryGetComponent(out Bonus bonus))
                return;

            if(bonus.coinsToAdd != 0)
            {
                _player.Coins += bonus.coinsToAdd;
                SoundManager.Instance.PlayCoinPickUp();
            }

            if (bonus is Rocket)
            {
                GetItemFromPool(PoolType.RocketPickUpFX);
                EventHub.OnBonusRocketPickUp();
            }

            if (bonus is Coin)
            {
                GetItemFromPool(PoolType.CoinPickUpFX);
            }
                
            other.gameObject.SetActive(false);
        }

        private PoolItem GetItemFromPool(PoolType type)
        {
            var item = PoolManager.Get(type);
            item.transform.position = _player.transform.position;
            item.transform.SetParent(_player.transform);
            item.gameObject.SetActive(true);
            return item;
        }
    }
}