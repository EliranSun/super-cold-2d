using UnityEngine;

public class RespawnOnDeath : MonoBehaviour
{
    public void Respawn()
    {
        var randomPosition = Random.insideUnitCircle * 5;
        var newGameObject = Instantiate(gameObject, randomPosition, Quaternion.identity);
        newGameObject.SetActive(true);
    }
}