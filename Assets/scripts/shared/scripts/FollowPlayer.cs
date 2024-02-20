using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Update()
    {
        var newPosition = player.transform.position;
        newPosition.z = transform.position.z;

        transform.position = newPosition;
    }
}