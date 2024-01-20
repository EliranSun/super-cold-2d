using System.Collections.Generic;
using action_triggers.scripts;
using observer.scripts;
using TMPro;
using UnityEngine;

public class CollectWeapon : WeaponActionsObserverSubject
{
    private const int TimeToCollect = 3;
    public bool isTriggered;
    public bool targetAcquired;
    [SerializeField] private TextMeshProUGUI weaponPickupText;

    // "Player" | "Enemy" = tag
    private readonly Dictionary<string, WeaponObserverEvents> _actions = new()
    {
        { "Player", WeaponObserverEvents.PlayerCollected },
        { "Enemy", WeaponObserverEvents.EnemyCollectedWeapon }
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
            if (weaponPickupText)
                weaponPickupText.text = $"{TimeToCollect - _timeOnTarget}";
        }
        else
        {
            if (!CanCollectWeapon())
                return;

            _weaponTarget.parent = transform;
            _weaponTarget.transform.rotation = Quaternion.identity;
            // _weaponTarget.transform.localScale = Vector3.one;

            var collectedBy = gameObject.tag;
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