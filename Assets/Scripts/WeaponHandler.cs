using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform playerTransform;
    private readonly int _radius = 4;

    private void Update()
    {
        PositionWeapon();
        if (Input.GetMouseButtonDown(0)) ShootBullet();
    }

    private void PositionWeapon()
    {
        var mouseTransform = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var toMouse = (mouseTransform - playerTransform.position).normalized;
        var targetPosition = playerTransform.position + toMouse * _radius;

        transform.position = new Vector3(targetPosition.x, targetPosition.y, -1);
        // rotate the z axis to look at the mouse
        var direction = mouseTransform - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void ShootBullet()
    {
        Instantiate(
            bulletPrefab,
            (Vector2)transform.position + Vector2.left * 1.5f,
            transform.rotation
        );
    }
}