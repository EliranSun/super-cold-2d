using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
internal class Dialogue
{
    [FormerlySerializedAs("Audio")] public AudioClip audio;
    [FormerlySerializedAs("Text")] public string text;
    public float waitInMs;
    public DialogueTrigger trigger;
    public Guid ID = Guid.NewGuid();

    public Dialogue(string text, AudioClip audio, DialogueTrigger trigger, float waitInMs = 0)
    {
        this.text = text;
        this.audio = audio;
        this.waitInMs = waitInMs;
        this.trigger = trigger;
    }
}

public enum BaseDialogueTrigger
{
}

public enum DialogueTrigger
{
    None,
    LevelRestart,
    LevelStart,
    PlayerDies,
    EnemyDies,
    RestartedAnyway,
    ShotGreenManForTheFirstTime,
    GreenManKilled
}

public class DialogueConfig : MonoBehaviour
{
    [SerializeField] private int startDialogueId;
    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private int activeDialogueIndex;

    private void Awake()
    {
        activeDialogueIndex = startDialogueId;
    }

    public void OnNotify(DialogueTrigger trigger)
    {
        foreach (var dialogue in dialogues)
            if (dialogue.trigger == trigger)
                ReadLine(dialogue);
    }

    private void ReadLine(Dialogue line)
    {
    }
}