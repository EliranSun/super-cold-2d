using System.Collections;
using System.Linq;
using UnityEngine;

public class ManInGreenAttacks : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform[] positionPoints;
    [SerializeField] private GameObject firePrefab;

    private void Start()
    {
        StartCoroutine(TeleportToRandomPoint());
        Attack();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();
    }

    private void Attack()
    {
        var radiusRange = new[] { 5, 8, 10 };
        var fireballCountRange = new[] { 5, 10, 15 };
        var rotationSpeedRange = new[] { 60f, 90f, 120f };

        for (var i = 0; i < 3; i++)
        {
            var randomRadius = radiusRange[Random.Range(0, radiusRange.Length)];
            var randomFireballCount = fireballCountRange[Random.Range(0, fireballCountRange.Length)];
            var randomRotationSpeed = rotationSpeedRange[Random.Range(0, rotationSpeedRange.Length)];

            // filter out selected radius, count and speed
            radiusRange = radiusRange.Where(x => x != randomRadius).ToArray();
            fireballCountRange = fireballCountRange.Where(x => x != randomFireballCount).ToArray();
            rotationSpeedRange = rotationSpeedRange.Where(x => x != randomRotationSpeed).ToArray();

            EmanateFire(randomRadius, randomFireballCount, randomRotationSpeed);
        }
    }

    private IEnumerator TeleportToRandomPoint()
    {
        while (true)
        {
            var randomIndex = Random.Range(0, positionPoints.Length - 1);
            var randomPosition = positionPoints[randomIndex].position;
            transform.position = randomPosition;

            yield return new WaitForSeconds(5f);
        }
    }

    private void EmanateFire(int radius, int fireballCount, float rotationSpeed = 60f)
    {
        var fireParent = new GameObject("FireParent")
        {
            transform =
            {
                position = transform.position
            }
        };

        fireParent.AddComponent<RotateAndFireProjectiles>().SetSpeed(rotationSpeed);

        for (var i = 0; i < fireballCount; i++)
        {
            // Calculate angle in a circle for each firePrefab
            var angle = i * Mathf.PI * 2 / fireballCount;
            var position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            var fire = Instantiate(firePrefab, transform.position + position, Quaternion.identity);
            // Rotate firePrefab to face outward from the circle
            fire.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            fire.transform.SetParent(fireParent.transform);
        }
    }
}