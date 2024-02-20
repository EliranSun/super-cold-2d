using action_triggers.scripts;
using UnityEngine;

public class OnEnemyHitByProjectile : MonoBehaviour
{
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    [SerializeField] private GameObject pistol;
    private Animator _animator;
    private CollectWeapon _collectWeapon;
    private EnemyMovement _enemyMovement;
    private MoveToFixedPoints _moveToFixedPoints;
    private NpcShootBullets _npcShootBullets;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collectWeapon = GetComponent<CollectWeapon>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _npcShootBullets = GetComponent<NpcShootBullets>();
        _moveToFixedPoints = GetComponent<MoveToFixedPoints>();
    }

    public void OnNotify(WeaponObserverEvents observerEvent)
    {
        if (observerEvent == WeaponObserverEvents.EnemyHitByProjectile)
        {
            _animator.SetBool(IsDead, true);
            _enemyMovement.enabled = false;
            _npcShootBullets.enabled = false;
            _moveToFixedPoints.enabled = false;

            WeaponToss();

            StopAllCoroutines();
        }
    }

    private void WeaponToss()
    {
        if (_collectWeapon && _collectWeapon.targetAcquired)
        {
            pistol.transform.parent = null;
            pistol.GetComponent<LookAtPlayer>().enabled = false;
            // pistol.GetComponent<WeaponHandler>().simulated = true;
            pistol.GetComponentInChildren<SpinFast>().enabled = true;
        }
    }
}