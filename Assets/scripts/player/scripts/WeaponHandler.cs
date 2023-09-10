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
    private bool _isActive;

    private void Update()
    {
        if (!_isActive) return;

        PositionWeapon();

        if (Input.GetMouseButtonDown(0))
            ShootBullet();
    }

    public void OnNotify(string message)
    {
        if (message == WeaponActions.PlayerCollected.ToString())
            _isActive = true;

        if (message == WeaponActions.EnemyFiredShot.ToString())
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
        // newBullet.GetComponent<BulletForce>().isBouncy = isBulletBouncy;

        var rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z - 90);
        // should be in front of the weapon
        var position = transform.position + transform.right * 2;


        Instantiate(bulletPrefab, position, rotation);
        Invoke(nameof(Foo), 0.1f);
    }

    private void Foo()
    {
        print("bullet fired");
    }
}