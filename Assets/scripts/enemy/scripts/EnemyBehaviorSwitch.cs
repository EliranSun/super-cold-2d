using System;
using System.Collections;
using System.Collections.Generic;
using action_triggers.scripts;
using UnityEngine;

public class EnemyBehaviorSwitch : MonoBehaviour
{
    private EnemyMovement _enemyBasicMovementAndGunControlScript;
    private MoveToFixedPoints _moveToFixedPointsScript;

    private void Start()
    {
        _enemyBasicMovementAndGunControlScript = GetComponent<EnemyMovement>();
        _moveToFixedPointsScript = GetComponent<MoveToFixedPoints>();
    }

    public void OnNotify(WeaponObserverEvents weaponEvent)
    {
        if (weaponEvent == WeaponObserverEvents.EnemyCollectedWeapon)
        {
            _enemyBasicMovementAndGunControlScript.enabled = false;
            _moveToFixedPointsScript.enabled = true;
        }
    }
}
