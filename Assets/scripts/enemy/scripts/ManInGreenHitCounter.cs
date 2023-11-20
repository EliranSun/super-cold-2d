using TMPro;
using UnityEngine;

public class ManInGreenHitCounter : MonoBehaviour
{
    [SerializeField] private int hitTarget = 20;
    [SerializeField] private TextMeshProUGUI hitCountText;
    private int _hitCount;


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            _hitCount++;
            if (hitCountText) hitCountText.text = $"{(hitTarget - _hitCount).ToString()}";
            if (_hitCount >= hitTarget) Destroy(gameObject);
        }
    }
}