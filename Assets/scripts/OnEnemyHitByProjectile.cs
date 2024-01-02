using UnityEngine;

public class OnEnemyHitByProjectile : MonoBehaviour
{
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnNotify(WeaponActions action)
    {
        if (action == WeaponActions.HitByProjectile)
        {
            _animator.SetBool(IsDead, true);
            gameObject.GetComponent<EnemyMovement>().enabled = false;
            gameObject.GetComponent<NpcShootBullets>().enabled = false;
            StopAllCoroutines();
        }
    }
}