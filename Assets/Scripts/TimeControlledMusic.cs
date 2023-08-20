using UnityEngine;

public class TimeControlledMusic : MonoBehaviour
{
    private AudioSource _audioSource;
    private TimeController _timeController;

    private void Start()
    {
        _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audioSource.pitch = _timeController.isTimeSlowed ? 0.8f : 1f;
    }
}