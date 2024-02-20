using UnityEngine;
using Random = UnityEngine.Random;

public class SpinFast : MonoBehaviour
{
    private float _randomX;
    private float _randomY;
    private Rigidbody2D _rigidBody2D;

    private void OnEnable()
    {
        // TODO: Throw in the direction opposite of hit, to mimic the effect of being hit
        _randomX = Random.Range(-50, 50);
        _randomY = Random.Range(-50, 50);

        _rigidBody2D = GetComponent<Rigidbody2D>();

        _rigidBody2D.AddTorque(1000f, ForceMode2D.Force);
        _rigidBody2D.AddForce(new Vector2(_randomX, _randomY));
    }
}