using UnityEngine;

public class DestroyParentOnProjectileCollision : MonoBehaviour
{
    [SerializeField] private bool isEnabled;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isEnabled) return;

        if (col.gameObject.CompareTag("Bullet"))
            Destroy(gameObject.transform.parent.gameObject);
    }
}