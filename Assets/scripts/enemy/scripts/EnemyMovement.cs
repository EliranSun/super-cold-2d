using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] public Transform target;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float slowedSpeed = 1f;
    [SerializeField] private bool isControllingTime;
    [SerializeField] private TimeController timeController;
    [SerializeField] private bool isStatic;
    [SerializeField] private bool isRunningAwayFromPlayer;
    private CharacterController _characterController;
    private CollectWeapon _collectWeapon;
    private Vector2 _direction;
    private bool _targetAcquired;
    private Vector2 _targetPosition;
    private TimeController _timeController;
    private float _timeOnTarget;

    private void Start()
    {
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
            if (IsOutOfBounds(_direction))
            {
                timeController.NormalTime();
                return;
            }

            _characterController.Move(_direction.normalized * (Time.deltaTime * speed));
            return;
        }

        LookAtPlayer();

        if (isRunningAwayFromPlayer) RunAwayFromPlayer();
        if (isStatic) return;

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

        _characterController.Move(-direction.normalized * (Time.deltaTime * speed));
    }

    private void LookAtPlayer()
    {
        if (!playerPosition) return;

        var isPlayerToTheLeft = playerPosition.position.x < transform.position.x;
        transform.localScale = isPlayerToTheLeft ? new Vector3(-8, 8, 1) : new Vector3(8, 8, 1);
    }

    private void ControlTime(bool isMoving)
    {
        if (isMoving)
        {
            if (isControllingTime && !timeController.isTimeSlowed) timeController.SlowTime();
        }
        else if (isControllingTime && timeController.isTimeSlowed)
        {
            timeController.NormalTime();
        }
    }

    private void MoveToTarget()
    {
        if (_collectWeapon.targetAcquired) return;

        if (target) _targetPosition = target.position;

        var position1 = (Vector2)transform.position;
        _direction = _targetPosition - position1;
        var distance = Vector2.Distance(_targetPosition, position1);

        if (distance <= 4.4f)
        {
            if (target) _collectWeapon.Trigger(target);
            else _targetPosition = GetRandomVectorWithinLevelBounds();
            return;
        }

        if (_timeController.isTimeSlowed)
            _characterController.Move(_direction.normalized * (Time.deltaTime * slowedSpeed));
        else
            _characterController.Move(_direction.normalized * (Time.deltaTime * speed));
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