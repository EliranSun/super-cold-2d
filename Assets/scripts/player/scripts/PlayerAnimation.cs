using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            InvokeDeath();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            InvokeDeath();
    }

    private void InvokeDeath()
    {
        Invoke(nameof(Death), 0.1f);
    }

    private void Death()
    {
        _animator.SetBool(IsDead, true);
    }
}