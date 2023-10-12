using UnityEngine;

public class PlayerMovement : ObserverSubject
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool isControllingTime;
    [SerializeField] private GameObject target;
    [SerializeField] private bool godMode;
    private CollectWeapon _collectWeapon;
    private CharacterController _controller;
    private bool _isDead;
    private Vector3 _originalScale;
    private Rigidbody2D _rigidbody;
    private TimeController _timeController;
    private bool _triggeredCollectWeapon;

    private void Start()
    {
        _collectWeapon = GetComponent<CollectWeapon>();
        _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        if (_isDead)
            return;

        var movementVector = MoveRigidbody();
        ControlTime(movementVector);
        DetectNearWeapon();
    }

    private void OnDisable()
    {
        if (isControllingTime)
            ControlTime(Vector2.zero);
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

        if (col.gameObject.CompareTag("Bullet"))
        {
            _isDead = true;
            var directionOfHit = transform.position - col.transform.position;
            var force = directionOfHit.normalized * 100f;
            _rigidbody.AddForce(force);
            _timeController.isTimeSlowed = true;
            NotifyObservers(PlayerActions.IsDead);
        }
    }

    private void ControlTime(Vector2 movementVector)
    {
        if (movementVector != Vector2.zero)
        {
            if (isControllingTime && !_timeController.isTimeSlowed) _timeController.SlowTime();
        }
        else if (isControllingTime && _timeController.isTimeSlowed)
        {
            _timeController.NormalTime();
        }
    }

    private Vector2 MoveCharacter()
    {
        var normalizedSpeed = speed * Time.deltaTime;
        var movementVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) movementVector += Vector2.up * normalizedSpeed;
        if (Input.GetKey(KeyCode.A)) movementVector += Vector2.left * normalizedSpeed;
        if (Input.GetKey(KeyCode.S)) movementVector += Vector2.down * normalizedSpeed;
        if (Input.GetKey(KeyCode.D)) movementVector += Vector2.right * normalizedSpeed;

        if (movementVector != Vector2.zero)
        {
            if (!isControllingTime && _timeController.isTimeSlowed) _controller.Move(movementVector * 0.1f);
            else _controller.Move(movementVector);
        }

        return movementVector;
    }

    private Vector2 MoveRigidbody()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        if ((moveHorizontal > 0 && transform.localScale.x < 0) || (moveHorizontal < 0 && transform.localScale.x > 0))
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );

        var movement = new Vector2(moveHorizontal, moveVertical);
        _rigidbody.velocity = movement * speed;
        return movement;
    }

    private Vector2 MoveTransform()
    {
        var normalizedSpeed = speed * Time.deltaTime;
        var movementVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) movementVector += Vector2.up * normalizedSpeed;
        if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
            movementVector += Vector2.left * normalizedSpeed;
        }

        if (Input.GetKey(KeyCode.S)) movementVector += Vector2.down * normalizedSpeed;
        if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = _originalScale;
            movementVector += Vector2.right * normalizedSpeed;
        }

        if (movementVector != Vector2.zero)
        {
            if (!isControllingTime && _timeController.isTimeSlowed) transform.Translate(movementVector * 0.1f);
            else transform.Translate(movementVector);
        }

        return movementVector;
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