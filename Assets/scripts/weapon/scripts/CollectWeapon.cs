using TMPro;
using UnityEngine;

public class CollectWeapon : ObserverSubject
{
    private const int TimeToCollect = 3;
    public bool isTriggered;
    public bool targetAcquired;
    [SerializeField] private TextMeshProUGUI weaponPickupText;
    private float _timeOnTarget;
    private Transform _weaponTarget;

    private void Update()
    {
        if (!isTriggered)
        {
            _timeOnTarget = 0;
            if (weaponPickupText) weaponPickupText.text = "";
            return;
        }

        if (_timeOnTarget <= TimeToCollect)
        {
            _timeOnTarget += Time.deltaTime;
            if (weaponPickupText) weaponPickupText.text = $"{TimeToCollect - _timeOnTarget}";
        }
        else
        {
            if (gameObject.name == "Enemy") CollectEnemy();
            else if (gameObject.name == "Player") CollectPlayer();
        }
    }

    private void CollectPlayer()
    {
        if (!_weaponTarget) return;
        _weaponTarget.parent = transform;
        NotifyObservers(WeaponActions.PlayerCollected);
        ResetParams();
    }

    private void CollectEnemy()
    {
        if (!_weaponTarget) return;

        _weaponTarget.parent = transform;
        NotifyObservers(WeaponActions.EnemyCollected);
        ResetParams();
    }

    private void ResetParams()
    {
        _timeOnTarget = 0;
        targetAcquired = true;
        isTriggered = false;
        if (weaponPickupText) weaponPickupText.text = "";
    }

    public void Trigger(Transform target)
    {
        isTriggered = true;
        _weaponTarget = target;
    }
}