using UnityEngine;

public class ValeryBehaviourAtDeathSequence : ActionableScript
{
    [SerializeField] private GameObject valeryGameObject;

    public override void Activate()
    {
        valeryGameObject.GetComponent<MoveToFixedPoints>().enabled = true;
        Invoke(nameof(IncreaseSpeed), 5);
        Invoke(nameof(IncreaseSpeed), 10);
    }

    private void IncreaseSpeed()
    {
        valeryGameObject.GetComponent<MoveToFixedPoints>().speed *= 2;
        valeryGameObject.GetComponent<MoveToFixedPoints>().setNewPositionInterval /= 2;
    }
}