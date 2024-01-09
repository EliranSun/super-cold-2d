using action_triggers.scripts;
using UnityEngine;

public class OnEnemyHitByProjectile : MonoBehaviour
{
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnNotify(WeaponObserverEvents observerEvent)
    {
        if (observerEvent == WeaponObserverEvents.EnemyHitByProjectile)
        {
            _animator.SetBool(IsDead, true);
            gameObject.GetComponent<EnemyMovement>().enabled = false;
            StopAllCoroutines();
        }
    }
}