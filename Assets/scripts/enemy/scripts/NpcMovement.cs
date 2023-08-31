using System.Collections;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private TimeController timeController;
    private readonly int _speed = 1;
    private CharacterController _characterController;
    private Vector3 _randomDestinationWithinRadius;
    private int _slowDownFactor = 1;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _randomDestinationWithinRadius = Random.insideUnitSphere;
        print("randomDestinationWithinRadius: " + _randomDestinationWithinRadius);
        StartCoroutine(RestAndMove());
    }

    // Update is called once per frame
    private void Update()
    {
        _slowDownFactor = timeController.isTimeSlowed ? 10 : 1;
        _characterController.Move(_randomDestinationWithinRadius * _speed / _slowDownFactor * Time.deltaTime);
    }

    private IEnumerator RestAndMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _randomDestinationWithinRadius = Vector3.zero;

            yield return new WaitForSeconds(1f);
            _randomDestinationWithinRadius = Random.insideUnitSphere;
        }
    }
}