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
        public BonusType type;
    
        private void OnEnable()
        {
            rotationSpeed = Random.Range(100f, 200f);
            if(flare)
                flare.SetActive(enableFlare);
        }

        private void FixedUpdate()
        {
            if(!enableRotation)
                return;
        
            transform.Rotate(0f, rotationSpeed * Time.fixedDeltaTime, 0f);
        }
    }
}