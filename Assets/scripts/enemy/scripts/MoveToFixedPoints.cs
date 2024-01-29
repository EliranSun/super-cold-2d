using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class MoveToFixedPoints : CharacterDataObserverSubject
{
    [SerializeField] private GameObject currentPoint;
    [SerializeField] private GameObject[] graphPoints;
    
    [SerializeField] public float speed = 15;
    [SerializeField] public float setNewPositionInterval = 4;
    private CharacterController _characterController;
    private Vector3 _lastPosition;
    private Vector3 _newPosition;
    private bool _setNewPositionInvoked;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        transform.position = currentPoint.transform.position;
        
        Invoke(nameof(ChooseRandomPointInGraph), setNewPositionInterval);
    }

    private void Update()
    {
        // var distance = GetDistance();
        //
        // if (distance <= 1.3f && !_setNewPositionInvoked)
        // {
        //     Notify(CharacterData.IsIdle);
        //     Invoke(nameof(SetNewPosition), setNewPositionInterval);
        //     _setNewPositionInvoked = true;
        // }
        // else
        // {
        //     if (distance > 2 && _setNewPositionInvoked)
        //     {
        //         Notify(CharacterData.IsWalking);
        //         _setNewPositionInvoked = false;
        //     }
        //
        //     MoveToPosition();
        // }
        
        MoveToPosition();
    }
    
    void ChooseRandomPointInGraph()
    {
        var connectedPoints = currentPoint.GetComponent<GraphPoint>().connectedPoints;
        var randomIndex = Random.Range(0, connectedPoints.Count);
        var randomPoint = connectedPoints[randomIndex];
        
        SetNewPosition(randomPoint.transform.position);
        currentPoint = randomPoint;
    }

    private Vector3 GetRandomPosition(Vector3 lastPosition)
    {
        var newPositions = graphPoints.Where(x => x.transform.position != lastPosition).ToArray();
        var randomIndex = Random.Range(0, newPositions.Length - 1);
        return newPositions[randomIndex].transform.position;
    }
    
    private void MoveToPosition()
    {
        if (_newPosition == Vector3.zero)
            return;
        
        var direction = _newPosition - transform.position;
        var motion = direction.normalized * (Time.deltaTime * speed);
        motion.z = 0; // no motion for the z axis

        if (GetDistance() <= 0.3f)
        {
            _newPosition = Vector3.zero;
            Invoke(nameof(ChooseRandomPointInGraph), setNewPositionInterval);
            return;
        }
        
        _characterController.Move(motion);
    }

    private float GetDistance()
    {
        return Vector3.Distance(_newPosition, transform.position);
    }

    private void SetNewPosition(Vector3 newPosition)
    {
        _newPosition = newPosition;
    }
    
    private void SetNewPosition()
    {
        _newPosition = GetRandomPosition(_lastPosition);
        _lastPosition = _newPosition;
    }
}