using System.Collections;
using UnityEngine;

public class CarMovement : ObserverSubject
{
    private static readonly int IsDead = Animator.StringToHash("Died");
    [SerializeField] private Transform destination;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float speed = 10f;
    [SerializeField] private TimeController timeController;
    private readonly int _slowDownFactor = 20;
    private bool _isRespawning;

    private void Update()
    {
        if (_isRespawning) return;

        var isAtDestination = Vector3.Distance(transform.position, destination.position) < 0.1f;

        if (isAtDestination && !_isRespawning)
        {
            _isRespawning = true;
            StartCoroutine(RespawnCar());
            return;
        }

        if (destination)
        {
            var motion = (destination.position - transform.position).normalized;
            var randomSpeed = Random.Range(0.5f, 1f) * speed;

            if (timeController.isTimeSlowed)
            {
                transform.position += motion * (randomSpeed / _slowDownFactor * Time.deltaTime);
                return;
            }

            transform.position += motion * (randomSpeed * Time.deltaTime);
        }
    }

    private IEnumerator RespawnCar()
    {
        yield return new WaitForSeconds(Random.Range(1f, 8f));
        transform.position = respawnPoint.position;
        _isRespawning = false;
    }
}