using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
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
        if (GetComponent<CollectWeapon>().targetAcquired)
        {
            var pistolWrapper = GameObject.Find("Pistol");
            var pistol = GameObject.Find("Pistol/PistolWrapper");
            pistolWrapper.transform.parent = null;
            pistol.AddComponent<SpinFast>();
        }

        _animator.SetBool(IsDead, true);
        Invoke(nameof(RestartScene), 2f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}