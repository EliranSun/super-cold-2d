using System.Collections;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float scaleFactor;
    [SerializeField] private TimeController timeController;
    private readonly int _slowDownFactor = 20;
    private bool _isRespawning;
    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isRespawning) return;

        var isAtDestination = Vector3.Distance(transform.position, destination.position) < 0.1f;

        if (isAtDestination && !_isRespawning)
        {
            _isRespawning = true;
            // Invoke(nameof(RespawnCar), Random.Range(1f, 3f));
            StartCoroutine(RespawnCar());
            return;
        }

        if (destination)
        {
            var motion = (destination.position - transform.position).normalized;

            if (timeController.isTimeSlowed)
            {
                transform.position += motion * (speed / _slowDownFactor * Time.deltaTime);
                return;
            }

            transform.position += motion * (speed * Time.deltaTime);

            if (destination.position.y > transform.position.y) ScaleDownCar();
            else ScaleUpCar();
        }
    }

    private void ScaleDownCar()
    {
        transform.localScale *= scaleFactor;
    }

    private void ScaleUpCar()
    {
        transform.localScale /= scaleFactor;
    }

    private IEnumerator RespawnCar()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        print($"OS: {_originalScale}");
        transform.localScale = _originalScale;
        transform.position = respawnPoint.position;
        _isRespawning = false;
    }
}