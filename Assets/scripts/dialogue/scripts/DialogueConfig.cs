using System;
using System.Collections;
using config.scripts;
using enums;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
internal class Dialogue
{
    [FormerlySerializedAs("Audio")] public AudioClip audio;
    [FormerlySerializedAs("Text")] public string text;
    [FormerlySerializedAs("waitInMs")] public float waitInSeconds;
    public DialogueTrigger trigger;
    public GameObject[] options;
    public bool includesPlayerName;

    public Dialogue(string text, AudioClip audio, DialogueTrigger trigger, GameObject[] options,
        bool includesPlayerName, float waitInSeconds = 0.5f)
    {
        this.text = text;
        this.audio = audio;
        this.waitInSeconds = waitInSeconds;
        this.trigger = trigger;
        this.options = options;
        this.includesPlayerName = includesPlayerName;
    }
}


public class DialogueConfig : MonoBehaviour
{
    [SerializeField] private int allPlayerInfoDialogueIndex;
    [SerializeField] private int noPlayerInfoDialogueIndex;
    [SerializeField] private int noPlayerGenderDialogueIndex;
    [SerializeField] private TextMeshProUGUI dialogueLineContainer;
    [SerializeField] private int activeDialogueIndex;
    [SerializeField] private Dialogue[] dialogues;
    private AudioSource _audioSource;

    private PlayerGender _playerGender;
    private string _playerName;
    private float _time;
    private float _timeToReadCurrentLine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerGender = PlayerInfo.GetPlayerGender();
        _playerName = PlayerInfo.GetPlayerName();
        activeDialogueIndex = GetDialogueIndexBasedOnNameAndGender(_playerName, _playerGender);

        if (dialogues[activeDialogueIndex].trigger == DialogueTrigger.None)
            ReadLine(dialogues[activeDialogueIndex]);
    }

    private int GetDialogueIndexBasedOnNameAndGender(string playerName, PlayerGender gender)
    {
        int lineIndex;
        if (playerName == "" && gender == PlayerGender.None) lineIndex = noPlayerInfoDialogueIndex;
        else if (playerName != "" && gender == PlayerGender.None) lineIndex = noPlayerGenderDialogueIndex;
        else lineIndex = allPlayerInfoDialogueIndex;

        return lineIndex;
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
        catch
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
        dialogueLineContainer.text = line.includesPlayerName
            ? line.text.Replace("{playerName}", _playerName)
            : line.text;


        if (line.includesPlayerName)
        {
            var formattedPlayerName = $"{char.ToUpper(_playerName[0])}{_playerName.Substring(1)}";
            var lineWithPlayerName = line.text.Replace("{playerName}", formattedPlayerName);

            dialogueLineContainer.text = lineWithPlayerName;

            var textToSpeechComponent = GetComponent<TextToSpeech>();
            textToSpeechComponent.ConvertAndWait(lineWithPlayerName, _playerGender, audioClip =>
            {
                _audioSource.clip = audioClip;
                _audioSource.Play();

                if (line.options.Length > 0)
                    foreach (var optionGameObject in line.options)
                        optionGameObject.SetActive(true);
                else
                    Invoke(nameof(TriggerNextLine), line.waitInSeconds);
            });
        }
        else
        {
            if (line.audio != null)
            {
                _audioSource.clip = line.audio;
                _audioSource.Play();
                yield return new WaitForSeconds(_audioSource.clip.length);
            }
            else
            {
                yield return new WaitForSeconds(_timeToReadCurrentLine);
            }

            if (line.options.Length > 0)
                foreach (var optionGameObject in line.options)
                    optionGameObject.SetActive(true);
            else
                Invoke(nameof(TriggerNextLine), line.waitInSeconds);
        }
    }

    public void TriggerNextLine()
    {
        activeDialogueIndex++;
        TriggerLine(activeDialogueIndex);
    }
}