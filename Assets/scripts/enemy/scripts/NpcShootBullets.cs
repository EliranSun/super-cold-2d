using System.Collections;
using UnityEngine;

public class NpcShootBullets : ObserverSubject
{
    [SerializeField] private float shootInterval = 4f;
    private bool _isPlayerDead;

    public void OnNotify(string message)
    {
        if (message == WeaponActions.EnemyCollected.ToString())
            Invoke(nameof(ShootBulletsRoutine), 1f);

        if (message == PlayerActions.IsDead.ToString())
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