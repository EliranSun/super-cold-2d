using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float timeScale = 1f;
    public bool isTimeSlowed;

    public void SlowTime()
    {
        timeScale = 0.1f;
        isTimeSlowed = true;
    }

    public void NormalTime()
    {
        timeScale = 1f;
        isTimeSlowed = false;
    }
}