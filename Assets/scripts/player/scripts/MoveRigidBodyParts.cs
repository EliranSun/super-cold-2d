using UnityEngine;

public class MoveRigidBodyParts : ObserverSubject
{
    private static readonly int IsWalkingHorizontal = Animator.StringToHash("IsWalkingHorizontal");
    [SerializeField] private float speed = 60f;
    [SerializeField] private SpriteRenderer[] bodyPartsSpriteRenderers;
    [SerializeField] private Animator[] bodyPartsAnimators;
    private bool _isWalking;
    private Rigidbody2D _rigidbody;
    private bool notifiedObservers;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
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

        if (_isWalking && !notifiedObservers)
        {
            NotifyObservers(DialogueTrigger.PlayerMoved);
            notifiedObservers = true;
        }
    }
}