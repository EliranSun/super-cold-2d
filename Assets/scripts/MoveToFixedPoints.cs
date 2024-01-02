using System.Linq;
using action_triggers.scripts;
using UnityEngine;

public class MoveToFixedPoints : CharacterDataObserverSubject
{
    [SerializeField] private Transform[] positions;
    [SerializeField] private float speed = 15;
    private CharacterController _characterController;
    private Vector3 _lastPosition;
    private Vector3 _newPosition;
    private bool _setNewPositionInvoked;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        SetNewPosition();
    }

    private void Update()
    {
        var distance = GetDistance();

        if (distance <= 1.3f && !_setNewPositionInvoked)
        {
            Notify(CharacterData.IsIdle);
            Invoke(nameof(SetNewPosition), 4);
            _setNewPositionInvoked = true;
        }
        else
        {
            if (distance > 2 && _setNewPositionInvoked)
            {
                Notify(CharacterData.IsWalking);
                _setNewPositionInvoked = false;
            }

            MoveToPosition();
        }
    }

    private Vector3 GetRandomPosition(Vector3 lastPosition)
    {
        var newPositions = positions.Where(x => x.position != lastPosition).ToArray();
        var randomIndex = Random.Range(0, newPositions.Length - 1);
        return newPositions[randomIndex].position;
    }

    private void MoveToPosition()
    {
        var direction = _newPosition - transform.position;
        var motion = direction.normalized * (Time.deltaTime * speed);
        motion.z = 0; // no motion for the z axis

        _characterController.Move(motion);
    }

    private float GetDistance()
    {
        return Vector3.Distance(_newPosition, transform.position);
    }

    private void SetNewPosition()
    {
        _newPosition = GetRandomPosition(_lastPosition);
        _lastPosition = _newPosition;
    }
}