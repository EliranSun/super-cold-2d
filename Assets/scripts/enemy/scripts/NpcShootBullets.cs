using System.Collections;
using observer.scripts;
using UnityEngine;

public class NpcShootBullets : WeaponActionsObserverSubject
{
    [SerializeField] private float shootInterval = 4f;
    private bool _isPlayerDead;

    public void OnNotify(WeaponActions message)
    {
        if (message == WeaponActions.EnemyCollected)
            Invoke(nameof(ShootBulletsRoutine), 1f);
    }

    public void OnNotify(string message)
    {
        if (message == PlayerActions.Died.ToString())
        {
            StopCoroutine(ShootBullets());
            _isPlayerDead = true;
        }
    }

    private void ShootBulletsRoutine()
    {
        StartCoroutine(ShootBullets());
    }

    private IEnumerator ShootBullets()
    {
        while (!_isPlayerDead)
        {
            NotifyObservers(WeaponActions.EnemyFiredShot);
            yield return new WaitForSeconds(shootInterval);
        }
    }
}