using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool isControllingTime;
    [SerializeField] private GameObject target;
    [SerializeField] private CollectWeapon collectWeapon;
    private Vector3 _originalScale;
    private TimeController _timeController;
    private bool _triggeredCollectWeapon;

    private void Start()
    {
        _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        var movementVector = Move();
        ControlTime(movementVector);
        DetectNearWeapon();
    }

    private void OnDisable()
    {
        if (isControllingTime) ControlTime(Vector2.zero);
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

    private Vector2 Move()
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
        if (!collectWeapon || _triggeredCollectWeapon || !target) return;

        var position1 = (Vector2)transform.position;
        var distance = Vector2.Distance(target.transform.position, position1);
        if (distance <= 4.4f && target)
        {
            collectWeapon.Trigger(target.transform);
            _triggeredCollectWeapon = true;
        }
    }
}