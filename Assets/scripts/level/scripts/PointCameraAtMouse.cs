using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PointCameraAtMouse : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] private int radius = 4;
    
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PositionAtRadiusInRelationTo(mousePosition); 
    }
    
    private void PositionAtRadiusInRelationTo(Vector3 mousePosition)
    {
        var toMouse = (mousePosition - playerTransform.position).normalized;
        var targetPosition = playerTransform.position + toMouse * radius;
        targetPosition.z = transform.position.z;
        
        transform.position = targetPosition;
    }
}
