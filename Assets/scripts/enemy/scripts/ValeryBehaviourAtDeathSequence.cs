using System;
using System.Collections;
using action_triggers.scripts;
using UnityEngine;

public class ValeryBehaviourAtDeathSequence : ActionableScript
{
    [SerializeField] private GameObject valeryGameObject;

    public override void Activate()
    {
        bool isSeenDeathSequence = PlayerPreferences.GetPlayerPrefValue(PlayerPrefsKeys.SeenUniverseDeathSequence);
        if (isSeenDeathSequence)
            return;

        valeryGameObject.GetComponent<MoveToFixedPoints>().enabled = true;
        StartCoroutine(IncreaseSpeed());
    }

    private IEnumerator IncreaseSpeed()
    {
        while (true)
        {
            var footsteps = valeryGameObject.GetComponent<Footsteps>();
            var move = valeryGameObject.GetComponent<MoveToFixedPoints>();

            if (move.setNewPositionInterval < 0.15f)
                break;

            move.speed *= 2;
            move.setNewPositionInterval /= 2;
            footsteps.interval /= 2;

            yield return new WaitForSeconds(3);
        }
    }
}