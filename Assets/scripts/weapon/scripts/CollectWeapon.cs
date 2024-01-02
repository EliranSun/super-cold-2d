using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectWeapon : ObserverSubject
{
    private const int TimeToCollect = 3;
    public bool isTriggered;
    public bool targetAcquired;
    [SerializeField] private TextMeshProUGUI weaponPickupText;

    private readonly Dictionary<string, WeaponActions> _actions = new()
    {
        { "Player", WeaponActions.PlayerCollected },
        { "Enemy", WeaponActions.EnemyCollected }
    };

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
            if (!CanCollectWeapon())
                return;

            _weaponTarget.parent = transform;
            _weaponTarget.transform.rotation = Quaternion.identity;
            // _weaponTarget.transform.localScale = Vector3.one;

            var collectedBy = gameObject.name;
            NotifyObservers(_actions[collectedBy]);

            ResetParams();
        }
    }

    private bool CanCollectWeapon()
    {
        return _weaponTarget && _weaponTarget.gameObject.activeInHierarchy;
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