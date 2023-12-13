using UnityEngine;

public class HideAfter : MonoBehaviour
{
    [SerializeField] private int seconds;

    private void Start()
    {
        if (seconds > 0) Invoke(nameof(SetInactive), seconds);
    }

    private void SetInactive()
    {
        gameObject.SetActive(false);
    }
}