using UnityEngine;

public class PerspectiveMovement : MonoBehaviour
{
    [SerializeField] private float angle;
    [SerializeField] private float sensitivity;
    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        var yPosition = transform.position.y;

        // scale the character x & y based on y position - the lower the value the lower the scale should be, 
        // as if the character moves away/closer to the camera

        transform.localScale = _originalScale * (1 - yPosition / sensitivity);
    }
}