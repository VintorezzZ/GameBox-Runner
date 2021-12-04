using System;
using UnityEngine;

namespace MyGame.Player
{
    public class MoveController : MonoBehaviour
    {
        [HideInInspector] public float horizontalInput;
        private CharacterController _charController;
        private float _gravity;
        public float gravityAmount = -20;

        [SerializeField] private float startSpeed = 10;
        [SerializeField] private float maxSpeed = 20;
        [SerializeField] private float strafeSpeed = 6;
        [SerializeField] private float acceleration = 0.1f;

        private Animator _animator;
        
        private float laneDistance = 2f;
        [SerializeField] private float targetLane;
        [SerializeField] private float currentLane = 1;
        private float x;
        public float Speed { get; private set; }

        private global::Player _player;
        private void Awake()
        {
            _charController = GetComponent<CharacterController>();
        }

        public void Init(global::Player player, Animator animator)
        {
            _player = player;
            Speed = GameSettings.Config.startSpeed;
            strafeSpeed = GameSettings.Config.strafeSpeed;
            acceleration = GameSettings.Config.acceleration;
            _animator = animator;
        }

        private void ProcessInputs()
        {
            if (Input.GetKeyDown(KeyCode.A) | Input.GetKeyDown(KeyCode.LeftArrow) | SwipeManager.swipeLeft)
            {
                ChangeLane(-1);
                if (targetLane > -1)
                    _animator.SetTrigger("left");
            }
            else if (Input.GetKeyDown(KeyCode.D) | Input.GetKeyDown(KeyCode.RightArrow) | SwipeManager.swipeRight)
            {
                ChangeLane(1);
                if (targetLane < 3)
                    _animator.SetTrigger("right");
            }
        
            if ((Input.GetButtonDown("Jump") | SwipeManager.swipeUp) && _charController.isGrounded)
            {
                Jump();
            }
        }

        public void Tick()
        {
            ProcessInputs();
            Move();
            SpeedControl();
        }
    
        private void Move()
        {
            if (_charController.isGrounded && _gravity < 0)
            {
                _gravity = 0f;
            }

            _gravity += gravityAmount * Time.deltaTime;
            
            var targetPos = (currentLane - 1) * laneDistance;
            x = Mathf.Lerp(x, targetPos, strafeSpeed);

            var xMovement = Vector3.right * (x - transform.position.x) * strafeSpeed;
            var yMovement = Vector3.up * _gravity;
            var zMovement = Vector3.forward * Speed;
            
            _charController.Move((xMovement + yMovement + zMovement) * Time.deltaTime);

        }

        void ChangeLane(int direction)
        {
            targetLane = currentLane + direction;

            if (targetLane < 0 || targetLane > 2) // Ignore, we are on the borders.
                return;

            currentLane = targetLane;
        }
    
        private void Jump()
        {
            _gravity += Mathf.Sqrt(0.8f * -3.0f * gravityAmount);
        }
    
        private void SpeedControl()
        {
            Speed += acceleration * Time.deltaTime;
            if (Speed > maxSpeed)
                Speed = maxSpeed;
        }
        
        private void CheckForCrossBends(Collider other)
        {
            if (other.gameObject.CompareTag("Cross left"))
            {
                transform.localRotation *= Quaternion.Euler(0, -90, 0);
            }
            else if (other.gameObject.CompareTag("Cross right"))
            {
                transform.localRotation *= Quaternion.Euler(0, 90, 0);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            CheckForCrossBends(other);
        }
    }
}
