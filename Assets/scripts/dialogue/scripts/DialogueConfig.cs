using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
internal class Dialogue
{
    [FormerlySerializedAs("Audio")] public AudioClip audio;
    [FormerlySerializedAs("Text")] public string text;
    public float waitInMs;
    public DialogueTrigger trigger;
    public string[] options;

    public Dialogue(string text, AudioClip audio, DialogueTrigger trigger, string[] options, float waitInMs = 0)
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
    [SerializeField] private GameObject dialogueOptionButtonPrefab;
    [SerializeField] private Transform dialogueOptionsContainer;
    [SerializeField] private int averageWordsPerMinute = 300;
    [SerializeField] private TextMeshProUGUI dialogueLineContainer;
    [SerializeField] private int startDialogueIndex;
    [SerializeField] private int activeDialogueIndex;
    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private GameObject nameSelectionPanel;
    [SerializeField] private GameObject genderSelectionPanel;
    private AudioSource _audioSource;
    private float _time;
    private float _timeToReadCurrentLine;

    private void Awake()
    {
        nameSelectionPanel.SetActive(false);
        genderSelectionPanel.SetActive(false);

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
        ReadLine(dialogues[index]);
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
        dialogueLineContainer.text = line.text;
        _timeToReadCurrentLine = (float)line.text.Length / averageWordsPerMinute * 60;
        if (_audioSource)
        {
            _audioSource.clip = line.audio;
            _audioSource.Play();
        }

        yield return new WaitForSeconds(_timeToReadCurrentLine);

        if (line.options.Length > 0)
            for (var i = 0; i < line.options.Length; i++)
            {
                var buttonObject = Instantiate(dialogueOptionButtonPrefab, dialogueOptionsContainer);
                buttonObject.transform.position = new Vector3(
                    buttonObject.transform.position.x,
                    buttonObject.transform.position.y - i * 50,
                    buttonObject.transform.position.z
                );
                buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = line.options[i];
                var button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    SetPlayerOption(line.options[i]);
                    TriggerNextLine();
                });
            }
        else
            activeDialogueIndex++;
    }

    private void SetPlayerOption(string option)
    {
        print($"Player selected option: {option}");
        if (option == "MALE" || option == "FEMALE")
            PlayerPrefs.SetString("PlayerGender", option);
    }

    public void TriggerNextLine()
    {
        activeDialogueIndex++;
        TriggerLine(activeDialogueIndex);
    }
}