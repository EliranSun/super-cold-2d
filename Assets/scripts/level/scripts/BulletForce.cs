using UnityEngine;

public class BulletForce : ObserverSubject
{
    private const int MaxHits = 15;
    [SerializeField] private LayerMask detectableObjects;
    public bool isBouncy = true;
    private readonly float _force = 3500f;
    private readonly float _slowMotionForce = 10f;
    private int _currentHits;
    private bool _isBulletSlowed;
    private Vector2 _originalVelocity = Vector2.zero;
    private Rigidbody2D _rigidbody;
    private TimeController _timeController;

    private void Start()
    {
        _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(transform.up * _force * _timeController.timeScale);

        // observers.AddListener(GameObject.Find("Player").GetComponent<PlayerAnimation>().OnNotify);
        // if (_timeController.isTimeSlowed) _rigidbody.velocity /= slowMotionForce;
    }

    private void Update()
    {
        ControlVelocity();

        // TODO: Combine that with slow motion ability (where the speed is below ten but we dont want to destroy)
        // if (_rigidbody.velocity.magnitude is > 0 and < 10) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        print("Bullet Collision Detected with " + col.gameObject.name);
        var bullet = gameObject;

        if (col.gameObject.CompareTag("ManInGreen"))
        {
            Destroy(bullet);
            return;
        }

        if (col.gameObject.CompareTag("Wall"))
        {
            if (!isBouncy)
            {
                Destroy(bullet);
                return;
            }

            _currentHits++;
            if (_currentHits >= MaxHits)
                Destroy(bullet);
            return;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(bullet);
            return;
        }

        // parent = enemy, which itself is CharacterController2D. The child has a normal collider.
        // TODO: not optimal I see no reason to try and fix it, or don't use the CC2D at all.
        // var parent = col.gameObject.transform.parent;
        // if (parent && parent.CompareTag("Enemy"))
        // {
        //     col.gameObject.transform.parent.GetComponent<ExplodeOnDeath>().TriggerDeath();
        //     Invoke(nameof(Respawn), 2);
        //     Destroy(bullet); // bullet
        // }

        Destroy(bullet);
        Destroy(col.gameObject);
    }

    private void Respawn()
    {
        // TODO: Unclear what is the purpose of this method.
        // Respawn a bullet after hit? maybe I've had a mistake here and I meant to respawn the enemy.
        // Potentially in the boss battle.
        gameObject.GetComponent<RespawnOnDeath>().Respawn();
    }

    private void ControlVelocity()
    {
        if (_timeController.isTimeSlowed)
        {
            if (_isBulletSlowed) return;

            _originalVelocity = _rigidbody.velocity;
            _rigidbody.velocity /= _slowMotionForce;
            _isBulletSlowed = true;
        }
        else
        {
            if (!_isBulletSlowed) return;
            if (_originalVelocity == Vector2.zero)
                _rigidbody.AddForce(transform.up * (_force * _timeController.timeScale));
            else
                _rigidbody.velocity = _originalVelocity;
            _isBulletSlowed = false;
        }
    }

    private Vector2 IsEnemyOnBulletPath()
    {
        var hit = Physics2D.Raycast(transform.position, transform.forward, 10f, detectableObjects);
        if (hit.collider == null) return Vector2.zero;

        var hitX = hit.collider.bounds.center.x - hit.point.x;
        var hitY = hit.collider.bounds.center.y - hit.point.y;

        return new Vector2(hitX, hitY);
    }

    public string GetDirection()
    {
        switch (_rigidbody.velocity.x)
        {
            case > 0:
                return "right";
            case < 0:
                return "left";
        }

        return _rigidbody.velocity.y switch
        {
            > 0 => "up",
            < 0 => "down",
            _ => "none"
        };
    }
}