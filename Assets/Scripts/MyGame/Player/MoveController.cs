using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MyGame.Player
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    public class MoveController : MonoBehaviour
    {
        private CharacterController _charController;
        private Animator _animator;
        private float _colliderBaseHeight;
        private Vector3 _colliderBaseCenter;
        private float _gravity;
        private readonly Timer _slideTimer = new Timer();
        private float _slideDuration = .5f;
        private Coroutine _slideRoutine;
        
        public float gravityAmount = -20;
        [SerializeField] private float airSlideGravityForce;
        [SerializeField] private float jumpForce;

        [HideInInspector] public float horizontalInput;
        [SerializeField] private float startSpeed = 10;
        [SerializeField] private float maxSpeed = 20;
        [SerializeField] private float strafeSpeed = 6;
        [SerializeField] private float acceleration = 0.1f;

        [SerializeField] private float laneDistance = 2f;
        private float _targetLane;
        private float _currentLane = 1;
        private float x;

        public float Speed { get; private set; }

        private global::Player _player;

        private void Awake()
        {
            _charController = GetComponent<CharacterController>();
            _colliderBaseHeight = _charController.height;
            _colliderBaseCenter = _charController.center;
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
                if (_targetLane > -1)
                {
                    _animator.SetBool("strafe mirror", false);
                    _animator.SetTrigger("strafe");
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) | Input.GetKeyDown(KeyCode.RightArrow) | SwipeManager.swipeRight)
            {
                ChangeLane(1);
                if (_targetLane < 3)
                {
                    _animator.SetBool("strafe mirror", true);
                    _animator.SetTrigger("strafe");
                }
            }
        
            if ((Input.GetButtonDown("Jump") | Input.GetKeyDown(KeyCode.W) | SwipeManager.swipeUp))
            {
                if(!_charController.isGrounded)
                    return;

                if (_slideTimer.IsStarted)
                {
                    StopCoroutine(_slideRoutine);
                    _slideTimer.Stop();
                    _charController.height = _colliderBaseHeight;
                    _charController.center = _colliderBaseCenter;
                }
                
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.S) | Input.GetKeyDown(KeyCode.DownArrow) | SwipeManager.swipeDown)
            {
                if(_slideTimer.IsStarted && _slideTimer.Time < _slideDuration)
                    return;
                
                Slide();
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
            _gravity += gravityAmount * Time.deltaTime;
            
            if (_charController.isGrounded && _gravity <= -1f)
                _gravity = -1f;

            var targetPos = (_currentLane - 1) * laneDistance;
            x = Mathf.Lerp(x, targetPos, strafeSpeed);

            var xMovement = Vector3.right * (x - transform.position.x) * strafeSpeed;
            var yMovement = Vector3.up * _gravity;
            var zMovement = Vector3.forward * Speed;
            
            _charController.Move((xMovement + yMovement + zMovement) * Time.deltaTime);
        }

        void ChangeLane(int direction)
        {
            _targetLane = _currentLane + direction;

            if (_targetLane < 0 || _targetLane > 2) // Ignore, we are on the borders.
                return;

            _currentLane = _targetLane;
        }
    
        private void Jump()
        {
            _gravity += Mathf.Sqrt(jumpForce * -3.0f * gravityAmount);
            _animator.SetTrigger("jump");
        }
    
        private void Slide()
        {
            _slideTimer.Start();
            
            if(!_charController.isGrounded)
                _gravity += airSlideGravityForce;
            
            _charController.height *= .5f;
            var newCenter = _charController.center;
            newCenter.y *= .5f;
            _charController.center = newCenter;
            _animator.SetTrigger("slide");
            
            if(_slideRoutine != null)
                StopCoroutine(_slideRoutine);
            
            _slideRoutine = StartCoroutine(SlideRoutine());
        }

        private IEnumerator SlideRoutine()
        {
            while (_slideTimer.IsStarted && _slideTimer.Time < _slideDuration)
            {
                yield return null;
            }
            
            _slideTimer.Stop();
            _charController.height = _colliderBaseHeight;
            _charController.center = _colliderBaseCenter;
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
