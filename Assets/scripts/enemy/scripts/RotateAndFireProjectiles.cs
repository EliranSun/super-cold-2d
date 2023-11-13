using UnityEngine;

public class RotateAndFireProjectiles : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    [SerializeField] private int rotationCount = 1;
    private bool _areProjectilesLaunched;
    private Transform[] _children;
    private float _totalRotation;

    private void Update()
    {
        RotateAndIncreaseSpeed();
    }

    private void RotateAndIncreaseSpeed()
    {
        print(_totalRotation);

        if (_totalRotation >= rotationCount * 360)
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
            child.GetComponent<Rigidbody2D>().velocity = child.up * 10f;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}