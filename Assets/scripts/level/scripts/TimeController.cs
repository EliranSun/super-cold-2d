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
        timeScaleText.text = "0.1";
    }

    public void NormalTime()
    {
        timeScale = 1f;
        isTimeSlowed = false;
        timeScaleText.text = "1";
    }
}