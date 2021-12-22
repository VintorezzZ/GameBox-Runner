using System.Collections;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

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
        public StuntController stuntController;
        [SerializeField] private float airSlideGravityForce;
        [SerializeField] private float jumpForce;
        
        [SerializeField] private float startSpeed = 10;
        [SerializeField] private float maxSpeed = 20;
        [SerializeField] private float strafeSpeed = 6;
        [SerializeField] private float acceleration = 0.1f;

        [SerializeField] private float laneDistance = 2f;
        private float _targetLane;
        private float _currentLane = 1;
        private int _lastHorizontalInput = 0;
        private float x;

        private bool _canJump = true;
        private bool _canSlide = true;
        public bool isRocketMovement = false;
        private float _cachedSpeed = 0;
        private float _rocketTargetHeight = 0;
        public float Speed { get; private set; }

        private global::Player _player;

        private void Awake()
        {
            _charController = GetComponent<CharacterController>();
            _colliderBaseHeight = _charController.height;
            _colliderBaseCenter = _charController.center;

            EventHub.bonusRocketPickedUp += OnBonusRocketPickedUp;
            EventHub.gameOvered += OnGameOvered;
        }

        private void OnDisable()
        {
            EventHub.bonusRocketPickedUp -= OnBonusRocketPickedUp;
            EventHub.gameOvered -= OnGameOvered;
        }

        public void Init(global::Player player, Animator animator)
        {
            _player = player;
            Speed = GameSettings.Config.startSpeed;
            strafeSpeed = GameSettings.Config.strafeSpeed;
            acceleration = GameSettings.Config.acceleration;
            _animator = animator;
            _animator.SetInteger("dance", Random.Range(0,4));
            stuntController.Init(animator);
        }

        private void ProcessInputs()
        {
            ProcessStrafe();
            ProcessJump();
            ProcessSlide();
        }

        private void ProcessSlide()
        {
            if (!Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.DownArrow) && !SwipeManager.swipeDown)
                return;

            if (!_canSlide || _slideTimer.IsStarted && _slideTimer.Time < _slideDuration)
                return;

            Slide();
        }

        private void ProcessJump()
        {
            if (!Input.GetButtonDown("Jump") 
                && !Input.GetKeyDown(KeyCode.W) 
                && !Input.GetKeyDown(KeyCode.UpArrow) 
                && !SwipeManager.swipeUp) 
                return;
            
            if(!_charController.isGrounded || !_canJump)
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

        private void ProcessStrafe()
        {
            if(!Input.GetKeyDown(KeyCode.A) 
               && !Input.GetKeyDown(KeyCode.D) 
               && !Input.GetKeyDown(KeyCode.LeftArrow) 
               && !Input.GetKeyDown(KeyCode.RightArrow) 
               && !SwipeManager.swipeLeft && !SwipeManager.swipeRight)
                return;

            int horizInput = (int)Input.GetAxisRaw("Horizontal");
            int sign = 0;

            if (Mathf.Abs(horizInput) > 0)
                sign = (int)Mathf.Sign(horizInput);
            else
                sign = SwipeManager.swipeLeft ? -1 : 1;
     
            if(!CanStrafe(sign))
                return;
            
            ChangeLane(sign);

            if (_targetLane > -1 && _targetLane < 3)
            {
                if(_charController.isGrounded 
                   && !_animator.IsInTransition(0) 
                   && _animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    _animator.SetBool("strafe mirror", sign < 0);
                    _animator.SetTrigger("strafe");
                }
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
            if(_charController.isGrounded)
                _animator.SetBool("isFlying", false);
            else if (_gravity <= -9f)
                _animator.SetBool("isFlying", true);
            
            var yMovement = Vector3.zero;
            
            if(!isRocketMovement)
            {
                _gravity += gravityAmount * Time.deltaTime;

                if (_charController.isGrounded && _gravity <= -1f)
                    _gravity = -1f;

                yMovement = Vector3.up * _gravity;
            }
            else
            {
                yMovement = Vector3.up * (_rocketTargetHeight - transform.position.y) * 3;
            }

            var targetPos = (_currentLane - 1) * laneDistance;
            x = Mathf.Lerp(x, targetPos, strafeSpeed);

            var xMovement = Vector3.right * (x - transform.position.x) * strafeSpeed;
            var zMovement = Vector3.forward * Speed;
            
            _charController.Move((xMovement + yMovement + zMovement) * Time.deltaTime);
        }

        private bool CanStrafe(int sign)
        {
            return !Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.right * sign, 1.5f);
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
            if (stuntController.canMakeTrick && _canJump)
            {
                stuntController.DoStunt();
                Debug.LogError("stunt");
            }            
            else
            {
                _animator.SetTrigger("jump");
                _animator.SetBool("isFlying", true);
                Debug.LogError("jump");
            }
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
            if(isRocketMovement)
                return;
            
            Speed += acceleration * Time.deltaTime;
            if (Speed > maxSpeed)
                Speed = maxSpeed;
        }
        
        private void OnBonusRocketPickedUp()
        {
            StartCoroutine(RocketMovement());
            
        }

        private IEnumerator RocketMovement()
        {
            _canJump = false;
            _canSlide = false;
            isRocketMovement = true;
            _cachedSpeed = Speed;
            Speed *= 3f;
            _gravity = 0;
            _rocketTargetHeight = transform.position.y + 5;
            
            _animator.ResetTrigger("jump");
            _animator.ResetTrigger("slide");
            _animator.ResetTrigger("strafe");
            
            yield return new WaitForSeconds(5f);
            
            _canJump = true;
            _canSlide = true;
            isRocketMovement = false;
            Speed = _cachedSpeed;
            _animator.SetBool("isFlying", true);
        }
        
        private void OnGameOvered()
        {
            StopAllCoroutines();
            _canJump = true;
            _canSlide = true;
            isRocketMovement = false;
            Speed = _cachedSpeed;
        }

    }
}
