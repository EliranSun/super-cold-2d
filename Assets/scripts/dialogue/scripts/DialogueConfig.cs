using System;
using System.Collections;
using action_triggers.scripts;
using config.scripts;
using enums;
using observer.scripts;
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
    public DialogueActions[] beforeLineActions;

    public string femaleText;
    public string maleText;
    public string robotText;

    public bool includesPlayerName;
    public string id = Guid.NewGuid().ToString();
    [SerializeField] public AudioGender audio;

    [FormerlySerializedAs("waitInSeconds")]
    public float waitForNextLineInSeconds = 0.5f;

    [FormerlySerializedAs("trigger")] public DialogueTrigger lineTriggeredBy;
    [FormerlySerializedAs("actions")] public DialogueActions[] afterLineActions;
    [FormerlySerializedAs("options")] public GameObject[] characterSelectionOptions;
}

[Serializable]
internal class DialogueActions
{
    public DialogueAction action;
    public Scenes selectedScene;
    public GameObject[] gameObjects;
    public ActionableScript[] actionableScripts;
}

internal enum Scenes
{
    None,
    UniverseDeathSequence,
    LevelOneApartment
}


public class DialogueConfig : DialogueObserverSubject
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

        var playerGender = PlayerPreferences.GetPlayerGender();
        var playerName = PlayerPreferences.GetPlayerName();
        var playerPartner = PlayerPreferences.GetPlayerPartner();

        if (activeDialogueIndex == 0)
        {
            var dialogueLineId = GetDialogueLineIdBasedOnNameAndGender(playerName, playerGender, playerPartner);
            var index = Array.FindIndex(dialogues, dialogue => dialogue.id == dialogueLineId);
            if (index != -1) activeDialogueIndex = index;
        }

        if (dialogues[activeDialogueIndex].lineTriggeredBy == DialogueTrigger.None)
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
            if (dialogue.lineTriggeredBy == trigger)
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
        InvokeAction(line.beforeLineActions);

        var partnerName = PlayerPreferences.GetPlayerPartner();
        var playerName = PlayerPreferences.GetPlayerName();
        var playerGender = PlayerPreferences.GetPlayerGender();

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
                WaitForInputOrTriggerNextLine(line, _audioSource.clip.length);
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
            WaitForInputOrTriggerNextLine(line);
        }
    }

    private string GetGenderLineText(Dialogue line)
    {
        var playerGender = PlayerPreferences.GetPlayerGender();
        return playerGender switch
        {
            PlayerGender.Male => line.maleText,
            PlayerGender.Female => line.femaleText,
            PlayerGender.None => line.robotText,
            _ => ""
        };
    }

    private AudioClip GetLineAudio(Dialogue line)
    {
        var playerGender = PlayerPreferences.GetPlayerGender();
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

    private void WaitForInputOrTriggerNextLine(Dialogue line, float currentClipLength = 0)
    {
        if (line.characterSelectionOptions.Length > 0)
        {
            foreach (var optionGameObject in line.characterSelectionOptions)
                optionGameObject.SetActive(true);
        }
        else
        {
            InvokeAction(line.afterLineActions);
            Invoke(nameof(TriggerNextLine), currentClipLength + line.waitForNextLineInSeconds);
        }
    }

    private void InvokeAction(DialogueActions[] actions)
    {
        foreach (var dialogueAction in actions)
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

                case DialogueAction.DeathSequenceStart:
                    PlayerPreferences.SetPlayerPrefValue(PlayerPrefsKeys.DeathSequenceEnded, false);
                    PlayerPreferences.SetPlayerPrefValue(PlayerPrefsKeys.SeenUniverseDeathSequence, false);
                    break;
                
                case DialogueAction.DeathSequenceEnd:
                case DialogueAction.PlayerCanMove:
                case DialogueAction.RevivePlayer:
                case DialogueAction.ReverseAnimation:
                    Notify(dialogueAction.action);
                    break;

                case DialogueAction.EnableActionableScript:
                    foreach (var actionableScript in dialogueAction.actionableScripts)
                        actionableScript.Activate();
                    break;

                case DialogueAction.ChangeScene:
                    if (dialogueAction.selectedScene == Scenes.None)
                        break;

                    var sceneName = dialogueAction switch
                    {
                        { selectedScene: Scenes.UniverseDeathSequence } => "UniverseDeathSequence",
                        { selectedScene: Scenes.LevelOneApartment } => "Level 1",
                        _ => ""
                    };

                    SceneManager.LoadScene(sceneName);
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
                dialogues[activeDialogueIndex].lineTriggeredBy != DialogueTrigger.None)
            {
                print("No more dialogues to trigger");
                return;
            }

            TriggerLine(activeDialogueIndex);
        }
    }
}