using System.Collections;
using action_triggers.scripts;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private float throttle = 0.5f;
    [SerializeField] private bool isNPC;
    [SerializeField] private AudioClip[] footsteps;
    private AudioSource _audioSource;
    private bool _isWalking;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayFootsteps());
    }

    private void Update()
    {
        if (isNPC)
            return;

        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        _isWalking = moveHorizontal != 0 || moveVertical != 0;
    }

    public void OnNotify(CharacterData info)
    {
        if (info == CharacterData.IsWalking) _isWalking = true;
        if (info == CharacterData.IsIdle) _isWalking = false;
    }

    private IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (!_isWalking)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            var randomIndex = Random.Range(0, footsteps.Length);
            _audioSource.clip = footsteps[randomIndex];
            _audioSource.Play();

            yield return new WaitForSeconds(throttle);
        }
    }
}