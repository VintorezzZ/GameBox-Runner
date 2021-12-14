using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Other
{
    public class CoinsSetup : MonoBehaviour
    {
        [SerializeField] private List<Coin> coins;

        public void Init()
        {
            DeActivate();
        }
        
        public void DeActivate()
        {
            foreach (var coin in coins)
            {
                coin.gameObject.SetActive(false);
            }
        }

        public void Activate()
        {
            foreach (var coin in coins)
            {
                coin.gameObject.SetActive(true);
            }
        }
    }
}