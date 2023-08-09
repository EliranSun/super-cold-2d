using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    public bool isTriggered;
    public bool targetAcquired;
    private float timeOnTarget;
    private Transform weaponTarget;

    private void Update()
    {
        if (!isTriggered)
        {
            timeOnTarget = 0;
            return;
        }

        timeOnTarget += Time.deltaTime;

        if (timeOnTarget >= 2)
        {
            Collect();
            timeOnTarget = 0;
            targetAcquired = true;
            isTriggered = false;
        }
    }

    public void Collect()
    {
        if (!weaponTarget) return;

        weaponTarget.parent = transform;
        weaponTarget.rotation = transform.rotation;
        weaponTarget.position = new Vector2(transform.position.x - 1, transform.position.y);
        GetComponent<NpcShootBullets>().enabled = true;
    }

    public void Trigger(Transform target)
    {
        isTriggered = true;
        weaponTarget = target;
    }
}