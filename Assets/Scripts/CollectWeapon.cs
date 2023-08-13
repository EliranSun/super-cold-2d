using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    public bool isTriggered;
    public bool targetAcquired;
    private float _timeOnTarget;
    private Transform _weaponTarget;

    private void Update()
    {
        if (!isTriggered)
        {
            _timeOnTarget = 0;
            return;
        }

        _timeOnTarget += Time.deltaTime;

        if (_timeOnTarget >= 2)
        {
            if (gameObject.name == "Enemy") CollectEnemy();
            else if (gameObject.name == "Player") CollectPlayer();
        }
    }

    private void CollectPlayer()
    {
        if (!_weaponTarget) return;
        _weaponTarget.parent = transform;
        _weaponTarget.GetComponent<WeaponHandler>().enabled = true;
        ResetParams();
    }

    private void CollectEnemy()
    {
        if (!_weaponTarget) return;

        _weaponTarget.parent = transform;
        _weaponTarget.GetComponent<LookAtPlayer>().enabled = true;
        GetComponent<NpcShootBullets>().enabled = true;

        ResetParams();
    }

    private void ResetParams()
    {
        _timeOnTarget = 0;
        targetAcquired = true;
        isTriggered = false;
    }

    public void Trigger(Transform target)
    {
        isTriggered = true;
        _weaponTarget = target;
    }
}