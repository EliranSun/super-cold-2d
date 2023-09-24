using UnityEngine;
using UnityEngine.Serialization;

public class DialogueController : MonoBehaviour
{
    [FormerlySerializedAs("DeathSequence")] [FormerlySerializedAs("isDeadSequence")] [SerializeField]
    private AudioClip[] deathSequence;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnNotify(string message)
    {
        if (message == PlayerActions.IsDead.ToString())
            Invoke(nameof(IsDeadSequence), 10f);
    }

    private void IsDeadSequence()
    {
        _audioSource.clip = deathSequence[0];
        _audioSource.Play();
    }
}