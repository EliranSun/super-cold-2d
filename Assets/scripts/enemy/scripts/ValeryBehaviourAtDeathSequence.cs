using System.Collections;
using UnityEngine;

public class ValeryBehaviourAtDeathSequence : ActionableScript
{
    [SerializeField] private GameObject valeryGameObject;

    public override void Activate()
    {
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