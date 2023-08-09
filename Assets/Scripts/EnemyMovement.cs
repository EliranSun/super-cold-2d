using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float slowedSpeed = 1f;
    private CharacterController _characterController;
    private CollectWeapon _collectWeapon;
    private bool _targetAcquired;
    private TimeController _timeController;
    private float _timeOnTarget;
    private Vector2 targetPosition;

    private void Start()
    {
        _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        _collectWeapon = GetComponent<CollectWeapon>();
        _characterController = GetComponent<CharacterController>();
        targetPosition = Random.insideUnitCircle * 5;
    }

    private void Update()
    {
        // LookAtPlayer();
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

    private void MoveToTarget()
    {
        if (_collectWeapon.targetAcquired) return;

        if (target) targetPosition = target.position;

        var position1 = (Vector2)transform.position;
        var direction = targetPosition - position1;
        var distance = Vector2.Distance(targetPosition, position1);

        if (distance <= 1f)
        {
            if (target) _collectWeapon.Trigger(target);
            else targetPosition = new Vector2(Random.Range(-30f, 30f), Random.Range(-15f, 15f));
            return;
        }

        if (_timeController.isTimeSlowed)
            _characterController.Move(direction.normalized * (Time.deltaTime * slowedSpeed));
        else
            _characterController.Move(direction.normalized * (Time.deltaTime * speed));
    }
}