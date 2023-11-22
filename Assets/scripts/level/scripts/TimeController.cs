using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeScaleText;
    public float timeScale = 1f;
    public bool isTimeSlowed;

    public void SlowTime()
    {
        timeScale = 0.1f;
        isTimeSlowed = true;
        if (timeScaleText) timeScaleText.text = "0.1";
    }

    public void NormalTime()
    {
        timeScale = 1f;
        isTimeSlowed = false;
        if (timeScaleText) timeScaleText.text = "1";
    }
}