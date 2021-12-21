using UnityEngine;

namespace MyGame.Other
{
    public class Bonus : MonoBehaviour
    {
        public int coinsToAdd;
        
        [SerializeField] private bool enableRotation;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private bool enableFlare;
        [SerializeField] private GameObject flare;
    
        private void OnEnable()
        {
            rotationSpeed = Random.Range(0.5f, 2);
            if(flare)
                flare.SetActive(enableFlare);
        }

        private void Update()
        {
            if(!enableRotation)
                return;
        
            transform.Rotate(0f, rotationSpeed, 0f);
        }
    }
}