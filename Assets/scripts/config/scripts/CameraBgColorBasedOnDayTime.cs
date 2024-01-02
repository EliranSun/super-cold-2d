using System;
using UnityEngine;

public class CameraBgColorBasedOnDayTime : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        if (DateTime.Now.Hour is >= 6 and < 12)
            mainCamera.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        else if (DateTime.Now.Hour is >= 12 and < 18)
            mainCamera.backgroundColor = new Color(1, 1, 1);
        else if (DateTime.Now.Hour is >= 18 and < 22)
            mainCamera.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
        else if (DateTime.Now.Hour >= 22 && DateTime.Now.Hour < 6)
            mainCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
    }
}