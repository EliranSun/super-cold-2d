using UnityEngine;

public class TimeControlledObject : MonoBehaviour
{
    private bool _isTimeSlowed;
    private Vector2 _originalVelocity = Vector2.zero;
    private Rigidbody2D _rigidbody2D;
    private TimeController _timeController;

    private void Start()
    {
        _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_timeController.isTimeSlowed && !_isTimeSlowed)
        {
            _isTimeSlowed = true;
            _originalVelocity = _rigidbody2D.velocity;
            _rigidbody2D.velocity *= _timeController.timeScale;
        }
        else if (!_timeController.isTimeSlowed && _isTimeSlowed)
        {
            _rigidbody2D.velocity = _originalVelocity;
            _isTimeSlowed = false;
        }
    }
}