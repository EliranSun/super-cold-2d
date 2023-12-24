using System;
using System.Collections;
using action_triggers.scripts;
using config.scripts;
using enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
internal class AudioGender
{
    [SerializeField] public AudioClip female;
    [SerializeField] public AudioClip male;
    [SerializeField] public AudioClip none;
}

[Serializable]
internal class Dialogue
{
    public string femaleText;
    [FormerlySerializedAs("Text")] public string text;
    [SerializeField] public AudioGender audio;
    public float waitInSeconds = 0.5f;
    public DialogueTrigger trigger;
    public DialogueAction action;
    public GameObject[] options;
    public bool includesPlayerName;
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
    private DialogueTrigger _queuedTrigger;
    private TextToSpeech _textToSpeechComponent;

    private float _time;
    private float _timeToReadCurrentLine;

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();

        _audioSource = GetComponent<AudioSource>();
        _textToSpeechComponent = GetComponent<TextToSpeech>();

        var playerGender = PlayerInfo.GetPlayerGender();
        var playerName = PlayerInfo.GetPlayerName();

        if (activeDialogueIndex == 0)
            activeDialogueIndex = GetDialogueIndexBasedOnNameAndGender(playerName, playerGender);

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

        var isImportantLine = trigger == DialogueTrigger.PlayerDied;

        var isLinePlaying = _audioSource.isPlaying;
        if (isLinePlaying && !isImportantLine)
        {
            _queuedTrigger = trigger;
            return;
        }

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

    public void TriggerLine(int index)
    {
        try
        {
            activeDialogueIndex = index;
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
        _audioSource.Stop();
        _audioSource.clip = null;
    }

    private void ReadLine(Dialogue line)
    {
        StartCoroutine(ReadLineCoroutine(line));
    }

    private IEnumerator ReadLineCoroutine(Dialogue line)
    {
        var partnerName = PlayerInfo.GetPlayerPartner();
        var playerName = PlayerInfo.GetPlayerName();
        var playerGender = PlayerInfo.GetPlayerGender();

        var lineText = GetGenderLineText(line);

        if (line.includesPlayerName)
        {
            var formattedPartnerName = $"{char.ToUpper(partnerName[0])}{partnerName.Substring(1)}";
            var formattedPlayerName = $"{char.ToUpper(playerName[0])}{playerName.Substring(1)}";
            var lineWithPlayerName = lineText.Replace("{playerName}", formattedPlayerName);
            lineWithPlayerName = lineWithPlayerName.Replace("{playerPartner}", formattedPartnerName);

            dialogueLineContainer.text = lineWithPlayerName;

            // TODO: Parse all {playerName} and {playerPartner} lines at the start of a scene, as fetching data
            // takes couple of seconds
            _textToSpeechComponent.ConvertText(lineWithPlayerName, playerGender, audioClip =>
            {
                PlayAudioSourceClip(audioClip);
                AfterLineActions(line);
            });
        }
        else
        {
            var lineAudio = GetLineAudio(line);
            dialogueLineContainer.text = lineText;

            if (lineAudio != null)
            {
                PlayAudioSourceClip(lineAudio);
                yield return new WaitForSeconds(_audioSource.clip.length);
            }
            else
            {
                yield return new WaitForSeconds(_timeToReadCurrentLine);
            }

            ClearLine();
            AfterLineActions(line);
        }

        InvokeAction(line);
    }

    private string GetGenderLineText(Dialogue line)
    {
        var playerGender = PlayerInfo.GetPlayerGender();
        return playerGender switch
        {
            PlayerGender.Male => line.text,
            PlayerGender.Female => line.femaleText == "" ? line.text : line.femaleText,
            PlayerGender.None => line.text,
            _ => ""
        };
    }

    private AudioClip GetLineAudio(Dialogue line)
    {
        var playerGender = PlayerInfo.GetPlayerGender();
        return playerGender switch
        {
            PlayerGender.Male => line.audio.male,
            PlayerGender.Female => line.audio.female,
            PlayerGender.None => line.audio.none,
            _ => null
        };
    }

    private void PlayAudioSourceClip(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    private void AfterLineActions(Dialogue line)
    {
        if (line.options.Length > 0)
            foreach (var optionGameObject in line.options)
                optionGameObject.SetActive(true);
        else
            Invoke(nameof(TriggerNextLine), line.waitInSeconds);
    }

    private void InvokeAction(Dialogue line)
    {
        switch (line.action)
        {
            default:
            case DialogueAction.None:
                break;

            case DialogueAction.NextScene:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;

            case DialogueAction.GameObjectActivation:
                break;
        }
    }

    public void TriggerNextLine()
    {
        if (_queuedTrigger != DialogueTrigger.None)
        {
            TriggerLine(_queuedTrigger);
            _queuedTrigger = DialogueTrigger.None;
        }
        else
        {
            activeDialogueIndex++;

            if (dialogues.Length <= activeDialogueIndex ||
                dialogues[activeDialogueIndex].trigger != DialogueTrigger.None)
            {
                print("No more dialogues to trigger");
                return;
            }

            TriggerLine(activeDialogueIndex);
        }
    }
}