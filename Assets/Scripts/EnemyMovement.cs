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
    private CharacterController _characterController;
    private CollectWeapon _collectWeapon;
    private bool _targetAcquired;
    private Vector2 _targetPosition;
    private TimeController _timeController;
    private float _timeOnTarget;
    private int count;
    private Vector2 direction;

    private void Start()
    {
        _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        _collectWeapon = GetComponent<CollectWeapon>();
        _characterController = GetComponent<CharacterController>();
        _targetPosition = Random.insideUnitCircle * 5;

        direction = _targetPosition - (Vector2)transform.position;
        if (isControllingTime) StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        if (isControllingTime) return;

        MoveToTarget();
    }

    private void LookAtPlayer()
    {
        if (!playerPosition) return;

        // rotate z axis to look at player
        var direction = playerPosition.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
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
        direction = _targetPosition - position1;
        var distance = Vector2.Distance(_targetPosition, position1);

        if (distance <= 4.4f)
        {
            if (target) _collectWeapon.Trigger(target);
            else _targetPosition = new Vector2(Random.Range(-30f, 30f), Random.Range(-15f, 15f));
            return;
        }

        if (_timeController.isTimeSlowed)
            _characterController.Move(direction.normalized * (Time.deltaTime * slowedSpeed));
        else
            _characterController.Move(direction.normalized * (Time.deltaTime * speed));
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            count++;
            _characterController.Move(direction.normalized * (Time.deltaTime * speed));
            ControlTime(true);
            yield return new WaitForSeconds(Random.Range(1, 4));
            _characterController.Move(Vector2.zero);
            ControlTime(false);
            direction = Random.insideUnitCircle * 5 - (Vector2)transform.position;
        }
    }
}