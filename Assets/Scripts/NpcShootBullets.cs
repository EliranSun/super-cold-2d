using System.Collections;
using UnityEngine;

public class NpcShootBullets : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 4f;
    [SerializeField] private Transform weaponTransform; // TODO: Actually get the current weapon transform

    private void OnEnable()
    {
        StartCoroutine(ShootBullets());
    }

    private IEnumerator ShootBullets()
    {
        while (true)
        {
            Instantiate(bulletPrefab, weaponTransform.position, weaponTransform.rotation);
            yield return new WaitForSeconds(shootInterval);
        }
    }
}