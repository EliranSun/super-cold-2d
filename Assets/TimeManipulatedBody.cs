using System.Collections.Generic;
using UnityEngine;

internal class PointInTime
{
    public Vector2 Position;
    public Quaternion Rotation;

    public PointInTime(Vector2 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}

public class TimeManipulatedBody : MonoBehaviour
{
    [SerializeField] private bool isInstantiated;
    public bool _isRewinding;
    private List<PointInTime> _pointsInTime;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _pointsInTime = new List<PointInTime>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartRewind();
        if (Input.GetKeyUp(KeyCode.Space)) StopRewind();
    }

    private void FixedUpdate()
    {
        if (_isRewinding)
            Rewind();
        else
            Record();
    }

    private void Rewind()
    {
        if (_pointsInTime.Count > 0)
        {
            var pointInTime = _pointsInTime[0];
            var transform1 = transform;
            transform1.position = pointInTime.Position;
            transform1.rotation = pointInTime.Rotation;
            _pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind(true);
        }
    }

    private void Record()
    {
        // if (_pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        // {
        //     _pointsInTime.RemoveAt(_pointsInTime.Count - 1);
        // }

        _pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    private void StartRewind()
    {
        _isRewinding = true;
        // _rigidbody.isKinematic = true;
    }

    private void StopRewind(bool isStartOfTime = false)
    {
        _isRewinding = false;
        // _rigidbody.isKinematic = false;

        if (isInstantiated && isStartOfTime)
            Destroy(gameObject);
    }
}