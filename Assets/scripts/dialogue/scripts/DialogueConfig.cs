using System;
using System.Collections;
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
    public GameObject[] options;

    public Dialogue(string text, AudioClip audio, DialogueTrigger trigger, GameObject[] options, float waitInMs = 0)
    {
        this.text = text;
        this.audio = audio;
        this.waitInMs = waitInMs;
        this.trigger = trigger;
        this.options = options;
    }
}


public class DialogueConfig : MonoBehaviour
{
    [SerializeField] private int averageWordsPerMinute = 300;
    [SerializeField] private TextMeshProUGUI dialogueLineContainer;
    [SerializeField] private int startDialogueIndex;
    [SerializeField] private int activeDialogueIndex;
    [SerializeField] private Dialogue[] dialogues;
    private AudioSource _audioSource;
    private float _time;
    private float _timeToReadCurrentLine;

    private void Awake()
    {
        activeDialogueIndex = startDialogueIndex;
        _audioSource = GetComponent<AudioSource>();

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

            if (dialogues[activeDialogueIndex].trigger == DialogueTrigger.None &&
                dialogues[activeDialogueIndex].options.Length == 0)
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

    private void TriggerLine(int index)
    {
        try
        {
            ReadLine(dialogues[index]);
        }
        catch (Exception e)
        {
            print("No more dialogues to read");
        }
    }

    private void ClearLine()
    {
        dialogueLineContainer.text = "";
    }

    private void ReadLine(Dialogue line)
    {
        StartCoroutine(ReadLineCoroutine(line));
    }

    private IEnumerator ReadLineCoroutine(Dialogue line)
    {
        _timeToReadCurrentLine = (float)line.text.Length / averageWordsPerMinute * 60;

        var isPlayerNameAtEndOfLine = line.text.Contains("{playerName}");
        var playerName = PlayerPrefs.GetString("PlayerName");

        if (isPlayerNameAtEndOfLine)
            dialogueLineContainer.text = line.text.Replace("{playerName}", playerName);
        else
            dialogueLineContainer.text = line.text;

        if (_audioSource)
        {
            _audioSource.clip = line.audio;
            _audioSource.Play();
        }

        yield return new WaitForSeconds(_timeToReadCurrentLine);
        if (isPlayerNameAtEndOfLine)
        {
            _timeToReadCurrentLine = ElevenLabsVoiceAPI.PlayerNameAudioClip.length;
            _audioSource.clip = ElevenLabsVoiceAPI.PlayerNameAudioClip;
            _audioSource.Play();
        }

        yield return new WaitForSeconds(_timeToReadCurrentLine);

        if (line.options.Length > 0)
            foreach (var optionGameObject in line.options)
                optionGameObject.SetActive(true);
        else
            activeDialogueIndex++;
    }

    public void TriggerNextLine()
    {
        activeDialogueIndex++;
        TriggerLine(activeDialogueIndex);
    }
}