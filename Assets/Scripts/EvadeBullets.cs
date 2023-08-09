using UnityEngine;

public class EvadeBullets : MonoBehaviour
{
    public Vector2 evadeDirection;
    public bool shouldEvade;
    private readonly float speed = 10f;
    private CharacterController characterController;
    private Vector2 randomDirection;

    private void Start()
    {
        characterController = transform.parent.gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (shouldEvade && IsWithinLevelBounds()) AvoidBullet();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var bullet = col.gameObject.GetComponent<BulletForce>();
        if (bullet)
        {
            shouldEvade = true;
            var direction = bullet.GetDirection();
            print(direction);
            switch (direction)
            {
                case "right":
                case "left":
                    randomDirection = Vector2.up * Random.Range(-5, 5);
                    break;

                case "up":
                case "down":
                    randomDirection = Vector2.right * Random.Range(-5, 5);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var bullet = other.gameObject.GetComponent<BulletForce>();
        if (bullet)
        {
            shouldEvade = false;
            randomDirection = Vector2.zero;
        }
    }

    private bool IsWithinLevelBounds()
    {
        if (transform.parent.position.x is > 16 or < -16 || transform.parent.position.y is > 8 or < -8)
            return false;

        return true;
    }


    private void AvoidBullet()
    {
        characterController.Move(randomDirection * (Time.deltaTime * speed));
    }
}