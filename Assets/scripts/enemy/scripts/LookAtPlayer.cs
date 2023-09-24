using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private const float NormalSpeed = 1f;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Transform playerTransform;
    private readonly int _radius = 3;
    private bool _isActive;

    private void Update()
    {
        if (!playerTransform || !_isActive) return;

        var toPlayer = (playerTransform.position - enemyTransform.position).normalized;
        var targetPosition = enemyTransform.position + toPlayer * _radius;

        transform.position = new Vector3(targetPosition.x, targetPosition.y, -1);

        // rotate the z axis to look at player
        var direction = playerTransform.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.FromToRotation(Vector3.forward, direction);

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void OnNotify(string message)
    {
        print(message);
        if (message == WeaponActions.EnemyCollected.ToString())
            _isActive = true;
    }
}