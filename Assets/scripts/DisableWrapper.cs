using UnityEngine;

public class DisableWrapper : MonoBehaviour
{
    public void Disable()
    {
        transform.parent.gameObject.SetActive(false);
    }
}