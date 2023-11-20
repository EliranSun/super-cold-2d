using UnityEngine;

public class SpinFast : MonoBehaviour
{
    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.AddTorque(1000f, ForceMode2D.Force);

        var randomX = Random.Range(-50, 50);
        var randomY = Random.Range(-50, 50);
        rb.AddForce(new Vector2(randomX, randomY));
    }
}