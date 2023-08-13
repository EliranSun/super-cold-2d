using System.Collections;
using UnityEngine;

public class NpcShootBullets : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 4f;
    [SerializeField] private Transform weaponTransform; // TODO: Actually get the current weapon transform
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        Invoke(nameof(ShootBulletsRoutine), 1f);
    }

    private void ShootBulletsRoutine()
    {
        StartCoroutine(ShootBullets());
    }

    private IEnumerator ShootBullets()
    {
        while (player != null)
        {
            Instantiate(
                bulletPrefab,
                (Vector2)weaponTransform.position + Vector2.left * 1.5f,
                weaponTransform.rotation
            );
            yield return new WaitForSeconds(shootInterval);
        }
    }
}