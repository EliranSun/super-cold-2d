using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float speed = 10f;
    private CharacterController _controller;
    private bool _isRespawning;
    private Vector3 _originalScale;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _originalScale = transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isRespawning) return;

        var isAtDestination = Vector3.Distance(transform.position, destination.position) < 0.1f;

        if (isAtDestination && !_isRespawning)
        {
            _isRespawning = true;
            Invoke(nameof(RespawnCar), Random.Range(1f, 3f));
            return;
        }

        if (destination)
        {
            var motion = (destination.position - transform.position).normalized;
            _controller.Move(motion * (speed * Time.deltaTime));
            ScaleDownCar();
        }
    }

    private void ScaleDownCar()
    {
        transform.localScale *= 0.998f;
    }

    private void ScaleUpCar()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.1f);
    }

    private void RespawnCar()
    {
        var carTransform = transform;
        carTransform.localScale = _originalScale;
        carTransform.position = respawnPoint.position;
        _isRespawning = false;
    }
}