using System.Collections;
using action_triggers.scripts;
using UnityEngine;

public class EnemyMovement : ObserverSubject
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] public Transform target;
    [SerializeField] public float speed = 15f;
    [SerializeField] private float slowedSpeed = 1f;
    [SerializeField] private bool isControllingTime;
    [SerializeField] private TimeController timeController;
    [SerializeField] private bool isStatic;
    [SerializeField] private bool isRunningAwayFromPlayer;
    private CharacterController _characterController;
    private CollectWeapon _collectWeapon;
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;
    private bool _targetAcquired;
    private Vector2 _targetPosition;
    private TimeController _timeController;
    private float _timeOnTarget;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        _collectWeapon = GetComponent<CollectWeapon>();
        _characterController = GetComponent<CharacterController>();
        _targetPosition = GetRandomVectorWithinLevelBounds();

        _direction = _targetPosition - (Vector2)transform.position;
        if (isControllingTime) StartCoroutine(ChangeMoveDirection());
    }

    private void Update()
    {
        if (isControllingTime)
        {
            if (IsOutOfBounds(_direction)) timeController.NormalTime();
            else _characterController.Move(_direction.normalized * (Time.deltaTime * speed));
            return;
        }

        LookAtPlayer();

        if (isRunningAwayFromPlayer)
            RunAwayFromPlayer();

        if (isStatic)
            return;

        MoveToTarget();
    }

    private void OnDisable()
    {
        if (isControllingTime) ControlTime(false);
    }

    private void RunAwayFromPlayer()
    {
        if (!playerPosition) return;

        var distanceFromPlayer = Vector2.Distance(playerPosition.position, transform.position);
        if (distanceFromPlayer > 20f) return;

        var direction = playerPosition.position - transform.position;
        if (IsOutOfBounds(direction)) return;

        MoveCharacterController(-direction.normalized * (Time.deltaTime * speed));
    }

    private void MoveCharacterController(Vector3 motion)
    {
        _characterController.Move(motion);
        NotifyObservers(CharacterData.IsWalking);
    }

    private void LookAtPlayer()
    {
        if (!playerPosition) return;

        var isPlayerToTheLeft = playerPosition.position.x < transform.position.x;
        if ((isPlayerToTheLeft && !_spriteRenderer.flipX) || (!isPlayerToTheLeft && _spriteRenderer.flipX))
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }


    private void ControlTime(bool isMoving)
    {
        if (isMoving && isControllingTime && !timeController.isTimeSlowed) timeController.SlowTime();
        else if (isControllingTime && timeController.isTimeSlowed) timeController.NormalTime();
    }

    private void MoveToTarget()
    {
        if (target && _collectWeapon.targetAcquired) return;

        if (target)
            _targetPosition = target.position;

        var position1 = (Vector2)transform.position;
        _direction = _targetPosition - position1;
        var distance = Vector2.Distance(_targetPosition, position1);

        if (distance <= 3f)
        {
            if (target) _collectWeapon.Trigger(target);
            else Invoke(nameof(SetNextRandomTarget), 4);

            NotifyObservers(CharacterData.IsIdle);
            return;
        }

        if (_timeController.isTimeSlowed)
            MoveCharacterController(_direction.normalized * (Time.deltaTime * slowedSpeed));
        else
            MoveCharacterController(_direction.normalized * (Time.deltaTime * speed));
    }

    private void SetNextRandomTarget()
    {
        _targetPosition = GetRandomVectorWithinLevelBounds();
    }

    public void OnNotify(PlayerActions message)
    {
        if (message == PlayerActions.Died)
            target = null;
        
        if (message == PlayerActions.SeenUniverseDeathSequence)
            isStatic = true;
    }
    
    public void OnNotify(DialogueAction message)
    {
        if (message == DialogueAction.DeathSequenceEnd)
            isStatic = false;
    }

    private Vector2 GetRandomVectorWithinLevelBounds()
    {
        var vec = new Vector2(Random.Range(-25f, 25f), Random.Range(-14f, 14f));
        return vec;
    }

    private bool IsOutOfBounds(Vector2 nextDirection)
    {
        var isDirectionUp = nextDirection.y > 0;
        var isDirectionDown = nextDirection.y < 0;
        var isDirectionLeft = nextDirection.x < 0;
        var isDirectionRight = nextDirection.x > 0;

        var isOutOfBoundsUp = transform.position.y >= 15f && isDirectionUp;
        var isOutOfBoundsDown = transform.position.y <= -15f && isDirectionDown;
        var isOutOfBoundsLeft = transform.position.x <= -30f && isDirectionLeft;
        var isOutOfBoundsRight = transform.position.x >= 30f && isDirectionRight;

        return isOutOfBoundsUp || isOutOfBoundsDown || isOutOfBoundsLeft || isOutOfBoundsRight;
    }

    private IEnumerator ChangeMoveDirection()
    {
        while (true)
        {
            _direction = Vector2.zero;
            ControlTime(false);
            yield return new WaitForSeconds(Random.Range(1, 4));
            ControlTime(true);
            _direction = GetRandomVectorWithinLevelBounds() - (Vector2)transform.position;
            yield return new WaitForSeconds(Random.Range(1, 4));
        }
    }
}