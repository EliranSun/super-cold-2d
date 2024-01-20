using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float slowDownScale = 0.1f;
    [SerializeField] private float superSlowDownScale = 0.07f;
    [SerializeField] private TextMeshProUGUI timeScaleText;
    public float timeScale = 1f;
    public bool isTimeSlowed;

    public void SlowTime()
    {
        timeScale = slowDownScale;
        isTimeSlowed = true;
        if (timeScaleText) timeScaleText.text = "0.1";
    }

    public void NormalTime()
    {
        timeScale = 1f;
        isTimeSlowed = false;
        if (timeScaleText) timeScaleText.text = "1";
    }

    public void SuperSlowTime()
    {
        timeScale = superSlowDownScale;
        isTimeSlowed = true;
        if (timeScaleText) timeScaleText.text = "0.01";
    }
}