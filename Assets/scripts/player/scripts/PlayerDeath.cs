using action_triggers.scripts;
using observer.scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : PlayerActionsObserverSubject
{
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    [SerializeField] private bool restartSceneOnDeath;
    [SerializeField] private bool disableCollisionOnDeath;
    [SerializeField] private TimeController timeController;
    private Animator _animator;
    private CollectWeapon _collectedWeapon;
    private Collider2D _collider2D;
    private Rigidbody2D _rigidbody;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _collectedWeapon = GetComponent<CollectWeapon>();
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
        Notify(PlayerActions.Died);

        if (_collectedWeapon && _collectedWeapon.targetAcquired)
        {
            var pistolWrapper = GameObject.Find("Pistol");
            var pistol = GameObject.Find("Pistol/PistolWrapper");
            pistolWrapper.transform.parent = null;
            pistol.AddComponent<SpinFast>();
        }

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.isKinematic = true;
        _animator.SetBool(IsDead, true);

        if (restartSceneOnDeath)
            Invoke(nameof(RestartScene), 2f);

        if (disableCollisionOnDeath)
            _collider2D.enabled = false;

        if (timeController)
            timeController.isTimeSlowed = true;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}