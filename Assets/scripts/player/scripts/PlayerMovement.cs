using action_triggers.scripts;
using observer.scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace player.scripts
{
    public class PlayerMovement : PlayerActionsObserverSubject
    {
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsVertical = Animator.StringToHash("isVertical");

        private static readonly int IsTurningBack = Animator.StringToHash("isTurningBack");

        [SerializeField] private bool isControllingTime;
        [SerializeField] private GameObject target;
        [SerializeField] private TimeController timeController;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float speed = 5f;

        private Animator _animator;
        private bool _areObserversNotified;
        private CollectWeapon _collectWeapon;
        private int _deathCount;
        private bool _isWalking;
        private Rigidbody2D _rigidbody;
        private bool _triggeredCollectWeapon;

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

            ControlTime();
            DetectNearWeapon();
            Move();
        }

        private void OnDisable()
        {
            if (isControllingTime) ControlTime();
        }

        private void Move()
        {
            var moveHorizontal = Input.GetAxis("Horizontal");
            var moveVertical = Input.GetAxis("Vertical");
            _isWalking = moveHorizontal != 0 || moveVertical != 0;

            var flipX = spriteRenderer.flipX;
            var isChangingDirectionToRight = moveHorizontal > 0 && flipX;
            var isChangingDirectionToLeft = moveHorizontal < 0 && !flipX;
            var isChangingDirectionToUp = moveVertical >= 0;

            if (isChangingDirectionToRight || isChangingDirectionToLeft)
                spriteRenderer.flipX = !spriteRenderer.flipX;

            _animator.SetBool(IsWalking, _isWalking);
            _animator.SetBool(IsVertical, moveVertical != 0);
            _animator.SetBool(IsTurningBack, isChangingDirectionToUp);

            var movement = new Vector2(moveHorizontal, moveVertical);
            _rigidbody.velocity = movement * speed;

            if (_isWalking && !_areObserversNotified)
            {
                NotifyObservers(PlayerActions.Moved);
                _areObserversNotified = true;
            }
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