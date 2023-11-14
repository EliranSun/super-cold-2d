using UnityEngine;

public class RotateAndFireProjectiles : MonoBehaviour
{
    private const float Force = 30f;
    private const int RotationCount = 1;
    private bool _areProjectilesLaunched;
    private Transform[] _children;
    private float _totalRotation;
    private float speed = 60f;

    private void Update()
    {
        RotateAndIncreaseSpeed();
    }

    private void RotateAndIncreaseSpeed()
    {
        if (_totalRotation >= RotationCount * 360)
        {
            if (!_areProjectilesLaunched)
            {
                LaunchProjectiles();
                _areProjectilesLaunched = true;
            }
        }
        else
        {
            var rotationThisFrame = speed * Time.deltaTime;
            transform.Rotate(0, 0, rotationThisFrame);
            _totalRotation += rotationThisFrame;
            speed += 0.01f;
        }
    }

    private void LaunchProjectiles()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<TimeControlledObject>();
            child.GetComponent<Rigidbody2D>().AddForce(child.up * Force, ForceMode2D.Impulse);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}