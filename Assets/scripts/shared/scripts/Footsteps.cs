using System.Collections;
using action_triggers.scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class Footsteps : MonoBehaviour
{
    [FormerlySerializedAs("throttle")] [SerializeField]
    public float interval = 0.5f;

    [SerializeField] private bool isNpc;
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private bool isEnabled = true;
    private AudioSource _audioSource;
    private bool _isWalking;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayFootsteps());
    }

    private void Update()
    {
        if (isNpc || !isEnabled)
            return;

        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        _isWalking = moveHorizontal != 0 || moveVertical != 0;
    }

    public void OnNotify(PlayerActions action)
    {
        if (action == PlayerActions.Died)
        {
            print("Footsteps disabled");
            isEnabled = false;
            StopCoroutine(PlayFootsteps());
        }
    }

    public void OnNotify(CharacterData data)
    {
        _isWalking = data switch
        {
            CharacterData.IsWalking => true,
            CharacterData.IsIdle => false,
            _ => _isWalking
        };
    }

    private IEnumerator PlayFootsteps()
    {
        while (isEnabled)
        {
            if (!_isWalking)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            var randomIndex = Random.Range(0, footsteps.Length);
            _audioSource.clip = footsteps[randomIndex];
            _audioSource.Play();

            yield return new WaitForSeconds(interval);
        }
    }
}