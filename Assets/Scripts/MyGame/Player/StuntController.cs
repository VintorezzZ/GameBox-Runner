using System.Collections;
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
            StartCoroutine(ResetStunt());
            canMakeTrick = false;
        }

        private IEnumerator ResetStunt()
        {
            yield return new WaitForSeconds(0.2f);
            _animator.SetInteger("trick", 0);
        }
    }
}