using UnityEngine;
using UnityEngine.SceneManagement;

namespace player.scripts
{
    public class PlayerMovement : ObserverSubject
    {
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsVertical = Animator.StringToHash("isVertical");
        private static readonly int IsTurningBack = Animator.StringToHash("isTurningBack");
        private static readonly int IsDead = Animator.StringToHash("IsDead");
        [SerializeField] private bool isControllingTime;
        [SerializeField] private GameObject target;
        [SerializeField] private TimeController timeController;
        [SerializeField] private bool isGodMode;
        [SerializeField] private bool isDead;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float speed = 5f;
        [SerializeField] private bool disableCollisionOnDeath;
        private Animator _animator;
        private bool _areObserversNotified;
        private CollectWeapon _collectWeapon;
        private int _deathCount;
        private bool _isWalking;
        private PolygonCollider2D _polygonCollider2D;
        private Rigidbody2D _rigidbody;
        private bool _triggeredCollectWeapon;

        private void Start()
        {
            _polygonCollider2D = GetComponent<PolygonCollider2D>();
            _collectWeapon = GetComponent<CollectWeapon>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            if (isDead) OnDeath();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (isDead)
                return;

            ControlTime();
            DetectNearWeapon();
            Move();
        }

        private void OnDisable()
        {
            if (isControllingTime) ControlTime();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            var hitByDeadlyObject = col.gameObject.CompareTag("Bullet") || col.gameObject.CompareTag("Car");

            if (hitByDeadlyObject && !isGodMode)
                OnDeath();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Bullet") && !isGodMode)
                OnDeath();
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
                NotifyObservers(DialogueTrigger.PlayerMoved);
                _areObserversNotified = true;
            }
        }

        private void OnDeath()
        {
            isDead = true;
            if (disableCollisionOnDeath) _polygonCollider2D.enabled = false;
            if (timeController) timeController.isTimeSlowed = true;

            _animator.SetBool(IsDead, true);

            NotifyObservers(PlayerActions.IsDead);
            NotifyObservers(DialogueTrigger.PlayerDied);
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