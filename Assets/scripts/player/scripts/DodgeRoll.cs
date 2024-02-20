using System.Collections;
using action_triggers.scripts;
using observer.scripts;
using UnityEngine;

public class DodgeRoll : PlayerActionsObserverSubject
{
    [SerializeField] private float rollDuration = 0.3f;
    private bool _isDodgeRolling;
    private BoxCollider2D _playerCollider;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_rb.velocity.normalized == Vector2.zero)
            return;

        if (Input.GetMouseButtonDown(1) && !_isDodgeRolling)
            StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        var elapsedTime = 0f;
        Notify(PlayerActions.DodgeRollingStarted);

        while (elapsedTime < rollDuration)
        {
            _isDodgeRolling = true;
            _playerCollider.enabled = false;
            var movementDirection = _rb.velocity.normalized;
            var angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            var rotationAmount = 360 * (Time.deltaTime / rollDuration);

            print(angle);

            switch (Mathf.Abs(angle))
            {
                case -90: // up
                    transform.Rotate(-rotationAmount, 0, 0);
                    break;

                case 90: // down
                    transform.Rotate(rotationAmount, 0, 0);
                    break;

                case 180: // left
                    transform.Rotate(0, 0, rotationAmount);
                    break;

                case 0: // right
                    transform.Rotate(0, 0, -rotationAmount);
                    break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ensure the character ends up facing the correct direction.
        transform.rotation = Quaternion.identity;
        _playerCollider.enabled = true;
        Notify(PlayerActions.DodgeRollingEnded);

        yield return new WaitForSeconds(1f);
        _isDodgeRolling = false;
    }
}