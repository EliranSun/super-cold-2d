using action_triggers.scripts;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake
    public Transform camTransform;

    // How long the object should shake for.
    private float _shakeDuration = 0;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 _originalPos;

    private void Awake()
    {
        if (camTransform == null)
            camTransform = GetComponent(typeof(Transform)) as Transform;
    }

    private void Update()
    {
        if (_shakeDuration > 0)
        {
            camTransform.localPosition = _originalPos + Random.insideUnitSphere * shakeAmount;
            _shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            _shakeDuration = 0f;
            camTransform.localPosition = _originalPos;
        }
    }

    private void OnEnable()
    {
        _originalPos = camTransform.localPosition;
    }

    public void OnNotify(PlayerActions playerAction)
    {
        print("CameraShake OnNotify");
        if (playerAction == PlayerActions.Died)
            _shakeDuration = 0.5f;
    }
}