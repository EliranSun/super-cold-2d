using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class MoveToFixedPoints : CharacterDataObserverSubject
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject currentPoint;
    [SerializeField] private GameObject[] graphPoints;
    [SerializeField] public float speed = 15;
    [SerializeField] public float setNewPositionInterval = 4;
    [SerializeField] private float walkInPathInterval = 4;
    private CharacterController _characterController;
    private GraphPoint _currentClosestPointToPlayer;
    private GraphPoint _lastClosestPointToPlayer;
    private Vector3 _lastPosition;
    private Vector3 _newPosition;
    private bool _setNewPositionInvoked;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        transform.position = currentPoint.transform.position;
        
        _currentClosestPointToPlayer = GetClosestPointToPlayer();
        var path = FindPath(currentPoint.GetComponent<GraphPoint>(), _currentClosestPointToPlayer);
        
        print($"START: {currentPoint.name} -> END: {_currentClosestPointToPlayer.name}");

        StartCoroutine(WalkInPath(path));
        StartCoroutine(UpdatePlayerClosestPoint());
    }

    private void Update()
    {
        MoveToPosition();
    }

    private GraphPoint GetClosestPointToPlayer()
    {
        var closestPoint = graphPoints[0];
        var closestDistance = Vector3.Distance(
            graphPoints[0].transform.position, 
            playerTransform.position
            );
        foreach (var point in graphPoints)
        {
            var distance = Vector3.Distance(point.transform.position, playerTransform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint.GetComponent<GraphPoint>();
    }

    private Vector3 GetRandomPosition(Vector3 lastPosition)
    {
        var newPositions = graphPoints
            .Where(x => x.transform.position != lastPosition)
            .ToArray();
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

    private static List<string> FindPath(GraphPoint start, GraphPoint target)
    {
        if (!start || !target)
            return null;

        var visited = new HashSet<GraphPoint>();
        var queue = new Queue<(GraphPoint Point, List<string> Path)>();
        queue.Enqueue((start, new List<string> { start.name }));

        while (queue.Count > 0)
        {
            var (current, path) = queue.Dequeue();

            if (visited.Contains(current))
                continue;

            if (current == target)
                return path;

            visited.Add(current);

            foreach (var neighbor in current.connectedPoints)
            {
                var newPath = new List<string>(path) { neighbor.name };
                queue.Enqueue((neighbor.GetComponent<GraphPoint>(), newPath));
            }
        }

        return null; // Path not found
    }

    private IEnumerator WalkInPath(List<string> path)
    {
        foreach (var point in path)
        {
            var isNextPointLast = path.IndexOf(point) == path.Count - 1;
            
            if (isNextPointLast)
            {
                // we want to stop the NPC before the last point,
                // so there would be some distance between the NPC and the player
                yield break;
            }
            
            var pointObject = graphPoints.FirstOrDefault(p => p.name == point);
            
            if (!pointObject)
                continue;

            SetNewPosition(pointObject.transform.position);
            currentPoint = pointObject;
            
            yield return new WaitForSeconds(walkInPathInterval);
        }
    }

    private IEnumerator UpdatePlayerClosestPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            _lastClosestPointToPlayer = _currentClosestPointToPlayer;
            _currentClosestPointToPlayer = GetClosestPointToPlayer();

            if (_currentClosestPointToPlayer != _lastClosestPointToPlayer)
            {
                var path = FindPath(currentPoint.GetComponent<GraphPoint>(), _currentClosestPointToPlayer);
                print($"START: {currentPoint.name} -> END: {_currentClosestPointToPlayer.name}");
                Debug.Log(string.Join(" -> ", path));

                StartCoroutine(WalkInPath(path));
            }
        }
    }
}