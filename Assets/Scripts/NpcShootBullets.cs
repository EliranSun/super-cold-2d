using System.Collections;
using UnityEngine;

public class NpcShootBullets : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 4f;
    [SerializeField] private Transform weaponTransform; // TODO: Actually get the current weapon transform
    [SerializeField] private GameObject player;
    [SerializeField] private bool isBulletBouncy = true;

    private void OnEnable()
    {
        Invoke(nameof(ShootBulletsRoutine), 1f);
    }

    private void ShootBulletsRoutine()
    {
        if (player == null)
        {
            StopCoroutine(ShootBullets());
            return;
        }

        StartCoroutine(ShootBullets());
    }

    private IEnumerator ShootBullets()
    {
        while (player != null)
        {
            var bulletPosition = weaponTransform.position + weaponTransform.up * 5;
            var newBullet = Instantiate(bulletPrefab, bulletPosition, weaponTransform.rotation);
            newBullet.GetComponent<BulletForce>().isBouncy = isBulletBouncy;
            yield return new WaitForSeconds(shootInterval);
        }
    }
}