using observer.scripts;
using UnityEngine;

public class NotifyCollision : WeaponActionsObserverSubject
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            NotifyObservers(WeaponActions.HitByProjectile);
    }
}