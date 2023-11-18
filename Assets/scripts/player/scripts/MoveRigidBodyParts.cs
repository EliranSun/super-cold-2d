using UnityEngine;

public class MoveRigidBodyParts : ObserverSubject
{
    private static readonly int IsWalkingHorizontal = Animator.StringToHash("IsWalkingHorizontal");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    [SerializeField] private float speed = 60f;
    [SerializeField] private SpriteRenderer[] bodyPartsSpriteRenderers;
    [SerializeField] private Animator[] bodyPartsAnimators;
    private bool _isWalking;
    private bool _notifiedObservers;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void OnDisable()
    {
        foreach (var bodyPartSpriteRenderer in bodyPartsSpriteRenderers)
            bodyPartSpriteRenderer.enabled = false;
    }

    private void Move()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        _isWalking = moveHorizontal != 0 || moveVertical != 0;

        foreach (var bodyPartSpriteRenderer in bodyPartsSpriteRenderers)
        {
            var flipX = bodyPartSpriteRenderer.flipX;
            var isChangingDirectionToRight = moveHorizontal > 0 && flipX;
            var isChangingDirectionToLeft = moveHorizontal < 0 && !flipX;

            if (isChangingDirectionToRight || isChangingDirectionToLeft)
                bodyPartSpriteRenderer.flipX = !bodyPartSpriteRenderer.flipX;
        }

        foreach (var bodyPartAnimator in bodyPartsAnimators)
            bodyPartAnimator.SetBool(IsWalkingHorizontal, _isWalking);


        var movement = new Vector2(moveHorizontal, moveVertical);
        _rigidbody.velocity = movement * speed;

        if (_isWalking && !_notifiedObservers)
        {
            NotifyObservers(DialogueTrigger.PlayerMoved);
            _notifiedObservers = true;
        }
    }
}