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
    [SerializeField] public AudioClip male;
    [SerializeField] public AudioClip female;
    [SerializeField] public AudioClip none;
}

[Serializable]
internal class Dialogue
{
    [FormerlySerializedAs("Text")] public string text;
    public string femaleText;
    public float waitInSeconds = 0.5f;
    public DialogueTrigger trigger;
    public DialogueAction action;
    public GameObject[] options;
    public bool includesPlayerName;
    [SerializeField] public AudioGender audio;
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

    private float _time;
    private float _timeToReadCurrentLine;

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        _audioSource = GetComponent<AudioSource>();
        PlayerGender playerGender = PlayerInfo.GetPlayerGender();
        string playerName = PlayerInfo.GetPlayerName();
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
        string playerName = PlayerInfo.GetPlayerName();
        PlayerGender playerGender = PlayerInfo.GetPlayerGender();
        
        var lineText = playerGender switch
        {
            PlayerGender.Male => line.text,
            PlayerGender.Female => line.femaleText == "" ? line.text : line.femaleText,
            PlayerGender.None => line.text,
            _ => ""
        };

        if (line.includesPlayerName)
        {
            var formattedPlayerName = $"{char.ToUpper(playerName[0])}{playerName.Substring(1)}";
            var lineWithPlayerName = lineText.Replace("{playerName}", formattedPlayerName);

            dialogueLineContainer.text = lineWithPlayerName;

            var textToSpeechComponent = GetComponent<TextToSpeech>();
            textToSpeechComponent.ConvertText(lineWithPlayerName, playerGender, audioClip =>
            {
                _audioSource.clip = audioClip;
                _audioSource.Play();

                if (line.options.Length > 0)
                    foreach (var optionGameObject in line.options)
                        optionGameObject.SetActive(true);
                else
                    Invoke(nameof(TriggerNextLine), _audioSource.clip.length + line.waitInSeconds);
            });
        }
        else
        {
            var audio = playerGender switch
            {
                PlayerGender.Male => line.audio.male,
                PlayerGender.Female => line.audio.female,
                PlayerGender.None => line.audio.none,
                _ => null
            };
            
            dialogueLineContainer.text = line.includesPlayerName
                ? lineText.Replace("{playerName}", playerName)
                : lineText;

            if (audio != null)
            {
                _audioSource.clip = audio;
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
        
        InvokeAction(line);
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

        }
    }

    public void TriggerNextLine()
    {
        activeDialogueIndex++;
        TriggerLine(activeDialogueIndex);
    }
}