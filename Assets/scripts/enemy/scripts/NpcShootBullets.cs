using System.Collections;
using UnityEngine;

public class NpcShootBullets : ObserverSubject
{
    [SerializeField] private float shootInterval = 4f;
    [SerializeField] private GameObject player;

    public void OnNotify(string message)
    {
        if (message == WeaponActions.EnemyCollected.ToString())
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
            NotifyObservers(WeaponActions.EnemyFiredShot);
            yield return new WaitForSeconds(shootInterval);
        }
    }
}