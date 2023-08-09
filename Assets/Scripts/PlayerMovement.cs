using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rigidBody2D;
    private TimeController timeController;

    private void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var normalizedSpeed = speed * Time.deltaTime;
        var movementVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) movementVector += Vector2.up * normalizedSpeed;
        if (Input.GetKey(KeyCode.A)) movementVector += Vector2.left * normalizedSpeed;
        if (Input.GetKey(KeyCode.S)) movementVector += Vector2.down * normalizedSpeed;
        if (Input.GetKey(KeyCode.D)) movementVector += Vector2.right * normalizedSpeed;

        if (movementVector != Vector2.zero)
        {
            transform.Translate(movementVector);
            if (!timeController.isTimeSlowed) timeController.SlowTime();
        }
        else
        {
            if (timeController.isTimeSlowed) timeController.NormalTime();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall")) transform.position = transform.position;
    }
}