using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
internal class Dialogue
{
    [FormerlySerializedAs("Audio")] public AudioClip audio;
    [FormerlySerializedAs("Text")] public string text;
    public float waitInMs;
    public int nextDialogueId;
    public DialogueTrigger trigger;

    public Dialogue(string text, AudioClip audio, float waitInMs = 0)
    {
        this.text = text;
        this.audio = audio;
        this.waitInMs = waitInMs;
    }

    public Dialogue(string text, AudioClip audio, int nextDialogueId, DialogueTrigger trigger, float waitInMs = 0)
    {
        this.text = text;
        this.audio = audio;
        this.waitInMs = waitInMs;
        this.nextDialogueId = nextDialogueId;
        this.trigger = trigger;
    }
}

public enum DialogueTrigger
{
    ShotGreenManForTheFirstTime
}

public class DialogueConfig : MonoBehaviour
{
    [SerializeField] private int startDialogueId;
    [SerializeField] private Dialogue[] dialogues;

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