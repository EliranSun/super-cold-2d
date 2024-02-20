using System.Collections;
using action_triggers.scripts;
using observer.scripts;
using UnityEngine;

public class NpcShootBullets : WeaponActionsObserverSubject
{
    [SerializeField] private bool isDisabled = false; 
    [SerializeField] private float shootInterval = 4f;
    private bool _isNpcDead;
    private bool _isPlayerDead;

    public void OnNotify(WeaponObserverEvents message)
    {
        if (isDisabled)
        {
            return;
        }
        
        if (message == WeaponObserverEvents.EnemyCollectedWeapon)
            Invoke(nameof(ShootBulletsRoutine), 1f);

        if (message == WeaponObserverEvents.EnemyHitByProjectile)
        {
            _isNpcDead = true;
            StopCoroutine(ShootBullets());
        }
    }

    public void OnNotify(PlayerActions message)
    {
        print($"NpcShootBullets OnNotify {message}");

        if (message == PlayerActions.Died)
        {
            _isPlayerDead = true;
            StopCoroutine(ShootBullets());
        }
    }

    private void ShootBulletsRoutine()
    {
        StartCoroutine(ShootBullets());
    }

    private IEnumerator ShootBullets()
    {
        while (!_isPlayerDead && !_isNpcDead)
        {
            NotifyObservers(WeaponObserverEvents.EnemyFiredShot);
            yield return new WaitForSeconds(shootInterval);
        }
    }
}