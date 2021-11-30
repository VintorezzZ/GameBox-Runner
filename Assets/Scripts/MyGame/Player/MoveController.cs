using UnityEngine;

namespace MyGame.Player
{
    public class MoveController : MonoBehaviour
    {
        [HideInInspector] public float horizontalInput;
        private CharacterController _characterController;
        private Vector3 _gravity;
        public float gravityAmount = -20;

        [SerializeField] private float startSpeed = 10;
        [SerializeField] private float maxSpeed = 20;
        [SerializeField] private float strafeSpeed = 6;
        [SerializeField] private float acceleration = 0.1f;

        public float Speed { get; private set; }

        private global::Player _player;
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Init(global::Player player)
        {
            _player = player;
            Speed = GameSettings.Config.startSpeed;
            strafeSpeed = GameSettings.Config.strafeSpeed;
            acceleration = GameSettings.Config.acceleration;
        }

        private void ProcessInputs()
        {
            horizontalInput = Input.GetAxis("Horizontal");
        
            if (Input.GetButtonDown("Jump") && _characterController.isGrounded)
            {
                Jump();
            }
        }

        public void Tick()
        {
            ProcessInputs();
            Move(horizontalInput);
            SpeedControl();
        }
    
        private void Move(float horizontalInput)
        {
            if (_characterController.isGrounded && _gravity.y < 0)
            {
                _gravity.y = 0f;
            }
        
            Vector3 moveDir = transform.right * (horizontalInput * strafeSpeed) + transform.forward * Speed;
            _characterController.Move(moveDir * Time.deltaTime);
        
            _gravity.y += gravityAmount * Time.deltaTime;
            _characterController.Move(_gravity * Time.deltaTime);
        }
    
        private void Jump()
        {
            _gravity.y += Mathf.Sqrt(0.8f * -3.0f * gravityAmount);
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
