using action_triggers.scripts;
using UnityEngine;

public class DeathSequenceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject deathSequenceStartDialogue;
    [SerializeField] private GameObject deathSequenceEndDialogue;
    [SerializeField] private GameObject currentSceneDialogue;
    [SerializeField] private int triggerAfterSeconds = 20;

    private void Start()
    {
        // TODO: initiate the death sequence end dialogue object - based on ... something
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