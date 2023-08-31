using TMPro;
using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    private const int timeToCollect = 3;
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

        if (_timeOnTarget <= timeToCollect)
        {
            _timeOnTarget += Time.deltaTime;
            if (weaponPickupText) weaponPickupText.text = $"{timeToCollect - _timeOnTarget}";
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
        // FIXME: Weapon cannot point to player due to cyclic dependency
        // _weaponTarget.GetComponent<WeaponHandler>().enabled = true;
        ResetParams();
    }

    private void CollectEnemy()
    {
        if (!_weaponTarget) return;

        _weaponTarget.parent = transform;
        // FIXME: Weapon cannot point to enemy due to cyclic dependency
        // _weaponTarget.GetComponent<LookAtPlayer>().enabled = true;
        // GetComponent<NpcShootBullets>().enabled = true;

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