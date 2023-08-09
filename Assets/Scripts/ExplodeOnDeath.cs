using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void TriggerDeath()
    {
        _particleSystem.Play();
        Destroy(gameObject, 1);
    }
}