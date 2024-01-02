using UnityEngine;

public class AlignWithPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Update()
    {
        transform.position = player.transform.position;
    }
}