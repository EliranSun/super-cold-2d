using action_triggers.scripts;
using observer.scripts;
using UnityEngine;

public class DeathSequenceTrigger : PlayerActionsObserverSubject
{
    [SerializeField] private GameObject deathSequenceStartDialogue;
    [SerializeField] private GameObject deathSequenceEndDialogue;
    [SerializeField] private GameObject currentSceneDialogue;
    [SerializeField] private int triggerAfterSeconds = 20;
    private PlayerInfo _playerInfo;

    private void Start()
    {
        _playerInfo = GetComponent<PlayerInfo>();
        if (_playerInfo)
        {
            var seenDeathSequence = _playerInfo.GetPlayerPrefValue(PlayerPrefsKeys.SeenUniverseDeathSequence);
            if (seenDeathSequence.ToUpper() == "TRUE")
            {
                currentSceneDialogue.SetActive(false);
                deathSequenceEndDialogue.SetActive(true);
                Notify(PlayerActions.SeenUniverseDeathSequence);
            }
        }
    }

    public void OnNotify(PlayerActions trigger)
    {
        if (trigger == PlayerActions.Died)
            Invoke(nameof(EnableDeathSequenceStartDialogue), triggerAfterSeconds);
    }

    private void EnableDeathSequenceStartDialogue()
    {
        currentSceneDialogue.SetActive(false);
        deathSequenceStartDialogue.SetActive(true);
    }
}