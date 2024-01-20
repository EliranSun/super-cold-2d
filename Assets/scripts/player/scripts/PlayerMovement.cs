using action_triggers.scripts;
using observer.scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace player.scripts
{
    public class PlayerMovement : PlayerActionsObserverSubject
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Walking = Animator.StringToHash("IsWalking");

        [SerializeField] private bool isControllingTime;
        [SerializeField] private GameObject target;
        [SerializeField] private TimeController timeController;
        [SerializeField] private float speed = 5f;
        [SerializeField] private bool isMovementEnabled = true;
        [SerializeField] private bool isGodMode;
        private Animator _animator;
        private bool _areObserversNotified;
        private CollectWeapon _collectWeapon;
        private int _deathCount;
        private bool _isWalking;
        private Vector2 _movement;
        private Rigidbody2D _rigidbody;
        private bool _triggeredCollectWeapon;
        private float lastHorizontal;
        private float lastVertical;

        private void Start()
        {
            _collectWeapon = GetComponent<CollectWeapon>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (isMovementEnabled)
            {
                ControlTime();
                DetectNearWeapon();
                RegisterMovementInput();
            }
        }

        private void FixedUpdate()
        {
            RigidBodyMove();
        }

        private void OnDisable()
        {
            if (isControllingTime) ControlTime();
        }

        public void OnNotify(PlayerActions action)
        {
            if (isGodMode)
                return;
            
            if (action == PlayerActions.Died)
                isMovementEnabled = false;
        }

        private void RegisterMovementInput()
        {
            _movement.x = Input.GetAxis("Horizontal");
            _movement.y = Input.GetAxis("Vertical");

            _isWalking = _movement.x != 0 || _movement.y != 0;

            if (_isWalking)
            {
                // Update the last direction moved for the correct idle animation
                lastHorizontal = _movement.x;
                lastVertical = _movement.y;
            }


            _animator.SetFloat(Horizontal, _isWalking ? _movement.x : lastHorizontal);
            _animator.SetFloat(Vertical, _isWalking ? _movement.y : lastVertical);
            _animator.SetBool(Walking, _isWalking);

            if (_isWalking && !_areObserversNotified)
            {
                Notify(PlayerActions.Moved);
                _areObserversNotified = true;
            }
        }

        private void RigidBodyMove()
        {
            var movement = new Vector2(_movement.x, _movement.y);
            _rigidbody.velocity = movement * speed;
        }

        private void ControlTime()
        {
            if (!timeController)
                return;

            if (_isWalking)
            {
                if (isControllingTime && !timeController.isTimeSlowed)
                    timeController.SlowTime();
            }
            else if (isControllingTime && timeController.isTimeSlowed)
            {
                timeController.NormalTime();
            }
        }

        private void DetectNearWeapon()
        {
            if (_triggeredCollectWeapon || !target) return;

            var position1 = (Vector2)transform.position;
            var distance = Vector2.Distance(target.transform.position, position1);
            if (distance <= 6f && target)
            {
                _collectWeapon.Trigger(target.transform);
                _triggeredCollectWeapon = true;
            }
        }
    }
}