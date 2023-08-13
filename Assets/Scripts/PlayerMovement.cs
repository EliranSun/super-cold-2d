using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool isControllingTime;
    [SerializeField] private GameObject target;
    [SerializeField] private CollectWeapon _collectWeapon;
    private TimeController timeController;

    private void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    private void Update()
    {
        var movementVector = Move();
        ControlTime(movementVector);
        DetectNearWeapon();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall")) transform.position = transform.position;
    }

    private void ControlTime(Vector2 movementVector)
    {
        if (movementVector != Vector2.zero)
        {
            if (isControllingTime && !timeController.isTimeSlowed) timeController.SlowTime();
        }
        else if (isControllingTime && timeController.isTimeSlowed)
        {
            timeController.NormalTime();
        }
    }

    private Vector2 Move()
    {
        var normalizedSpeed = speed * Time.deltaTime;
        var movementVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) movementVector += Vector2.up * normalizedSpeed;
        if (Input.GetKey(KeyCode.A)) movementVector += Vector2.left * normalizedSpeed;
        if (Input.GetKey(KeyCode.S)) movementVector += Vector2.down * normalizedSpeed;
        if (Input.GetKey(KeyCode.D)) movementVector += Vector2.right * normalizedSpeed;

        if (movementVector != Vector2.zero)
        {
            if (!isControllingTime && timeController.isTimeSlowed) transform.Translate(movementVector * 0.1f);
            else transform.Translate(movementVector);
        }

        return movementVector;
    }

    private void DetectNearWeapon()
    {
        if (!_collectWeapon) return;

        var position1 = (Vector2)transform.position;
        var distance = Vector2.Distance(target.transform.position, position1);
        if (distance <= 4.4f && target) _collectWeapon.Trigger(target.transform);
    }
}