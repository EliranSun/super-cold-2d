using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private const float SlowedDownSpeed = 0.3f;
    private const float NormalSpeed = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TimeController timeController;
    [SerializeField] private Camera mainCamera;

    private readonly int _radius = 4;

    private void Update()
    {
        PositionWeapon();

        if (Input.GetMouseButtonDown(0))
            ShootBullet();
    }

    private void PositionWeapon()
    {
        var mousePosition = GetMousePositionInWorld();
        PositionAtRadiusInRelationTo(mousePosition);
        LookAtMouse(mousePosition);
    }

    private void PositionAtRadiusInRelationTo(Vector3 mousePosition)
    {
        var playerPosition = playerTransform.position;
        var toMouse = (mousePosition - playerPosition).normalized;
        var targetPosition = playerPosition + toMouse * _radius;

        var speed = timeController.isTimeSlowed ? SlowedDownSpeed : NormalSpeed;
        var step = speed * Time.deltaTime;

        transform.position = timeController.isTimeSlowed
            ? Vector3.Lerp(transform.position, targetPosition, step)
            : new Vector3(targetPosition.x, targetPosition.y, -1);
    }

    private Vector3 GetMousePositionInWorld()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void LookAtMouse(Vector3 mousePosition)
    {
        var direction = mousePosition - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);

        var rotationSpeed =
            timeController.isTimeSlowed ? SlowedDownSpeed : NormalSpeed; // Change the rotation speed as needed
        var step = rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    }

    private void ShootBullet()
    {
        var transform1 = transform;
        Instantiate(
            bulletPrefab,
            (Vector2)transform1.position + Vector2.left * 1.5f,
            transform1.rotation
        );
    }
}