using System;
using config.scripts;
using TMPro;
using UnityEngine;

public class SetPlayerPref : MonoBehaviour
{
    [SerializeField] private float startTime = 0.3f; // poor
    [SerializeField] private float endTime = 0.79f; // she is so cold
    [SerializeField] private AudioSource fooSource;
    [SerializeField] private TextMeshProUGUI titleCard;
    [SerializeField] private AudioClip clip;
    private string _filePath;

    private void Start()
    {
        var playerName = GetPlayerName();
        if (playerName != null)
            TextToSpeechPlayerName(playerName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon))
            PlayPlayerName();
    }

    private void OnFetchComplete(AudioClip audioClip)
    {
        print("audioClip: " + audioClip);
        audioClip.name = GetPlayerName();
        fooSource.clip = audioClip;
        AudioClipUtility.SaveAudioClip(audioClip, _filePath);
    }

    public void PlayPlayerName()
    {
        fooSource.time = startTime;
        fooSource.Play();
        Invoke(nameof(StopSource), endTime - startTime);
    }

    private void StopSource()
    {
        fooSource.Stop();
    }

    private void TextToSpeechPlayerName(string playerName)
    {
        try
        {
            _filePath = Application.persistentDataPath + $"/{playerName}.wav";
            var loadedClip = AudioClipUtility.LoadAudioClip(_filePath);
            if (loadedClip == null)
                throw new Exception("clip not found");

            fooSource.clip = loadedClip;
            print($"loadedClip, {_filePath}");
            OnFetchComplete(loadedClip);
        }
        catch (Exception e)
        {
            print("clip not found, fetching");
            var upperCasePlayerName = $"{char.ToUpper(playerName[0])}{playerName.Substring(1)}";
            StartCoroutine(ElevenLabsVoiceAPI.VoiceGetRequest(upperCasePlayerName, OnFetchComplete));
        }
    }

    public void SetPlayerName(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
            return;

        PlayerPrefs.SetString("PlayerName", playerName);
        if (titleCard)
            titleCard.text = $"{playerName.ToUpper().Trim()} IS COLD";

        TextToSpeechPlayerName(playerName);
    }

    public void SetPlayerGender(string gender)
    {
        PlayerPrefs.SetString("PlayerGender", gender);
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName");
    }

    public static string GetPlayerGender()
    {
        return PlayerPrefs.GetString("PlayerGender");
    }
}