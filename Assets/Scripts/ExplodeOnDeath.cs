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
        Invoke(nameof(SetInactive), 1);
    }

    private void SetInactive()
    {
        gameObject.SetActive(false);
    }
}