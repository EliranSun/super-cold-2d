using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            _animator.SetBool(IsDead, true);
    }
}