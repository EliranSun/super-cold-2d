using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpinFast : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    float _randomX;
    float _randomY;

    private void Start()
    {
        _randomX = Random.Range(-50, 50);
        _randomY = Random.Range(-50, 50);    }

    private void OnEnable()
    {
        _rigidBody2D = gameObject.AddComponent<Rigidbody2D>();
        _rigidBody2D.gravityScale = 0;
        
        _rigidBody2D.AddTorque(1000f, ForceMode2D.Force);
        _rigidBody2D.AddForce(new Vector2(_randomX, _randomY));
    }
}