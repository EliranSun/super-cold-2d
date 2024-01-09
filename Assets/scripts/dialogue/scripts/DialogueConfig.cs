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

    [FormerlySerializedAs("text")] [FormerlySerializedAs("Text")]
    public string maleText;

    public bool includesPlayerName;
    public string id = Guid.NewGuid().ToString();
    [SerializeField] public AudioGender audio;
    public float waitInSeconds = 0.5f;
    public DialogueTrigger trigger;
    public DialogueActions[] actions;
    [FormerlySerializedAs("options")] public GameObject[] characterSelectionOptions;
}

[Serializable]
internal class DialogueActions
{
    public DialogueAction action;
    public GameObject[] gameObjects;
    public ActionableScript[] actionableScripts;
}


public class DialogueConfig : MonoBehaviour
{
    private static readonly int Reverse = Animator.StringToHash("Reverse");
    [SerializeField] private string noPlayerInfoDialogueId;
    [SerializeField] private string noPlayerGenderDialogueId;
    [SerializeField] private string noPlayerPartnerDialogueId;
    [SerializeField] private string allPlayerInfoDialogueId;
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
        _audioSource = GetComponent<AudioSource>();
        _textToSpeechComponent = GetComponent<TextToSpeech>();

        var playerGender = PlayerInfo.GetPlayerGender();
        var playerName = PlayerInfo.GetPlayerName();
        var playerPartner = PlayerInfo.GetPlayerPartner();

        if (activeDialogueIndex == 0)
        {
            var dialogueLineId = GetDialogueLineIdBasedOnNameAndGender(playerName, playerGender, playerPartner);
            var index = Array.FindIndex(dialogues, dialogue => dialogue.id == dialogueLineId);
            if (index != -1) activeDialogueIndex = index;
        }

        if (dialogues[activeDialogueIndex].trigger == DialogueTrigger.None)
            ReadLine(dialogues[activeDialogueIndex]);
    }

    private string GetDialogueLineIdBasedOnNameAndGender(string playerName, PlayerGender gender, string playerPartner)
    {
        string lineId;

        if (playerName == "") lineId = noPlayerInfoDialogueId;
        else if (gender == PlayerGender.None) lineId = noPlayerGenderDialogueId;
        else if (playerPartner == "") lineId = noPlayerPartnerDialogueId;
        else lineId = allPlayerInfoDialogueId;

        return lineId;
    }

    public void OnNotify(PlayerActions trigger)
    {
        if (trigger == PlayerActions.Died)
            OnNotify(DialogueTrigger.PlayerDied);
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
        InvokeAction(line);

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
                AfterLineActions(line, _audioSource.clip.length);
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
    }

    private string GetGenderLineText(Dialogue line)
    {
        var playerGender = PlayerInfo.GetPlayerGender();
        return playerGender switch
        {
            PlayerGender.Male => line.maleText,
            PlayerGender.Female => line.femaleText == "" ? line.maleText : line.femaleText,
            PlayerGender.None => line.maleText,
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

    private void AfterLineActions(Dialogue line, float currentClipLength = 0)
    {
        if (line.characterSelectionOptions.Length > 0)
            foreach (var optionGameObject in line.characterSelectionOptions)
                optionGameObject.SetActive(true);
        else
            Invoke(nameof(TriggerNextLine), currentClipLength + line.waitInSeconds);
    }

    private void InvokeAction(Dialogue line)
    {
        foreach (var dialogueAction in line.actions)
            switch (dialogueAction.action)
            {
                default:
                case DialogueAction.None:
                    break;

                case DialogueAction.NextScene:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;

                case DialogueAction.GameObjectsDisable:
                    foreach (var lineOptionGameObject in dialogueAction.gameObjects)
                        lineOptionGameObject.SetActive(false);
                    break;

                case DialogueAction.GameObjectsEnable:
                    foreach (var lineOptionGameObject in dialogueAction.gameObjects)
                        lineOptionGameObject.SetActive(true);
                    break;

                case DialogueAction.ReverseAnimation:
                    foreach (var dialogueActionGameObject in dialogueAction.gameObjects)
                        dialogueActionGameObject.GetComponent<Animator>().SetBool(Reverse, true);
                    break;

                case DialogueAction.EnableActionableScript:
                    foreach (var actionableScript in dialogueAction.actionableScripts)
                        actionableScript.Activate();
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