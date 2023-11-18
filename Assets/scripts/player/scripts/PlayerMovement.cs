using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : ObserverSubject
{
    [SerializeField] private bool isControllingTime;
    [SerializeField] private GameObject target;
    [SerializeField] private TimeController timeController;
    [SerializeField] private bool isGodMode;
    [SerializeField] private bool _isDead;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private CollectWeapon _collectWeapon;
    private int _deathCount;
    private bool _isWalking;
    private Rigidbody2D _rigidbody;
    private bool _triggeredCollectWeapon;

    private void Start()
    {
        _collectWeapon = GetComponent<CollectWeapon>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (_isDead)
            return;

        // TODO: Share with MoveRigidBodyParts.cs
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        _isWalking = moveHorizontal != 0 || moveVertical != 0;

        ControlTime();
        DetectNearWeapon();
    }

    private void OnDisable()
    {
        if (isControllingTime) ControlTime();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            transform.position = transform.position;

            if (col.gameObject.name == "GetLostBoundary")
                GameObject.Find("LevelManager").GetComponent<LevelManager>().AfraidToGetLost();

            if (col.gameObject.name == "CrossTheRoadBoundary")
                GameObject.Find("LevelManager").GetComponent<LevelManager>().AfraidToCrossTheRoad();

            if (col.gameObject.name == "NotHisHouseBoundary")
                GameObject.Find("LevelManager").GetComponent<LevelManager>().NotHisHouse();
        }

        if (col.gameObject.CompareTag("Bullet") && !isGodMode)
            DeclareDeath(col.collider);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet") && !isGodMode)
            DeclareDeath(col);
    }

    private void DeclareDeath(Collider2D collision)
    {
        _spriteRenderer.enabled = true;
        _isDead = true;
        var directionOfHit = transform.position - collision.transform.position;
        var force = directionOfHit.normalized * 100f;
        _rigidbody.AddForce(force);
        timeController.isTimeSlowed = true;
        GetComponent<MoveRigidBodyParts>().enabled = false;
        NotifyObservers(PlayerActions.IsDead);
    }

    private void ControlTime()
    {
        if (_isWalking)
        {
            if (isControllingTime && !timeController.isTimeSlowed)
                timeController.SlowTime();
        }
        else if (isControllingTime && timeController.isTimeSlowed)
        {
            timeController.NormalTime();
        }
    }

    private void DetectNearWeapon()
    {
        if (_triggeredCollectWeapon || !target) return;

        var position1 = (Vector2)transform.position;
        var distance = Vector2.Distance(target.transform.position, position1);
        if (distance <= 6f && target)
        {
            _collectWeapon.Trigger(target.transform);
            _triggeredCollectWeapon = true;
        }
    }
}