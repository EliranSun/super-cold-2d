using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private AudioClip[] isDeadSequence;
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
        _audioSource.clip = isDeadSequence[0];
        _audioSource.Play();
    }
}