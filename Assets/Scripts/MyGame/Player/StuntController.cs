using UnityEngine;

namespace MyGame.Player
{
    public class StuntController : MonoBehaviour
    {
        public bool canMakeTrick = false;
        private Animator _animator;

        public void Init(Animator animator)
        {
            _animator = animator;
        }
        
        public void Tick()
        {
            if(_animator.IsInTransition(0) && _animator.GetCurrentAnimatorStateInfo(0).IsTag("stunt"))
            {
                _animator.SetInteger("trick", 0);
                canMakeTrick = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            canMakeTrick = other.CompareTag("Trick trigger");
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Trick trigger"))
                canMakeTrick = false;
        }

        public void DoStunt()
        {
            _animator.SetInteger("trick", Random.Range(1, 4));
        }
    }
}