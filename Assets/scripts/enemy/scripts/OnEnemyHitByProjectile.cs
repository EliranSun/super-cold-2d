using action_triggers.scripts;
using UnityEngine;

public class OnEnemyHitByProjectile : MonoBehaviour
{
    [SerializeField] private GameObject pistol;
    CollectWeapon _collectWeapon;
    EnemyMovement _enemyMovement;
    NpcShootBullets _npcShootBullets;
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collectWeapon = GetComponent<CollectWeapon>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _npcShootBullets = GetComponent<NpcShootBullets>();
    }

    public void OnNotify(WeaponObserverEvents observerEvent)
    {
        if (observerEvent == WeaponObserverEvents.EnemyHitByProjectile)
        {
            _animator.SetBool(IsDead, true);
            _enemyMovement.enabled = false;
            _npcShootBullets.enabled = false;
            WeaponToss();
            
            StopAllCoroutines();
        }
    }

    void WeaponToss()
    {
        if (_collectWeapon && _collectWeapon.targetAcquired)
        {
            pistol.transform.parent = null;
            pistol.GetComponentInChildren<SpinFast>().enabled = true;
        }
    }
}