using System;
using TMPro;
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


public class DialogueConfig : MonoBehaviour
{
    [SerializeField] private int averageWordsPerMinute = 300;
    [SerializeField] private TextMeshProUGUI dialogueLineContainer;
    [SerializeField] private int startDialogueIndex;
    [SerializeField] private int activeDialogueIndex;
    [SerializeField] private Dialogue[] dialogues;
    private float _time;
    private float _timeToReadCurrentLine;

    private void Awake()
    {
        activeDialogueIndex = startDialogueIndex;

        if (dialogues[activeDialogueIndex].trigger == DialogueTrigger.None)
            ReadLine(dialogues[activeDialogueIndex]);
    }

    private void Update()
    {
        if (_timeToReadCurrentLine == 0)
            return;

        _time += Time.deltaTime;

        if (_time >= _timeToReadCurrentLine)
        {
            _time = 0;
            _timeToReadCurrentLine = 0;

            if (activeDialogueIndex >= dialogues.Length) return;

            if (dialogues[activeDialogueIndex].trigger == DialogueTrigger.None)
                ReadLine(dialogues[activeDialogueIndex]);
            else
                ClearLine();
        }
    }

    public void OnNotify(DialogueTrigger trigger)
    {
        print($"DialogueConfig OnNotify (DialogueTrigger) {trigger}");
        TriggerLine(trigger);
    }

    private void TriggerLine(DialogueTrigger trigger)
    {
        foreach (var dialogue in dialogues)
            if (dialogue.trigger == trigger)
            {
                activeDialogueIndex = Array.IndexOf(dialogues, dialogue);
                ReadLine(dialogue);
            }
    }

    private void ClearLine()
    {
        dialogueLineContainer.text = "";
    }

    private void ReadLine(Dialogue line)
    {
        dialogueLineContainer.text = line.text;
        _timeToReadCurrentLine = (float)line.text.Length / averageWordsPerMinute * 60;
        activeDialogueIndex++;
    }
}