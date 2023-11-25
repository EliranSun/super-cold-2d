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
    private string _filePath;

    private void Start()
    {
        try
        {
            _filePath = Application.persistentDataPath + "/playerNameAudioClip.wav";
            var loadedClip = AudioClipUtility.LoadAudioClip(_filePath);
            if (loadedClip == null)
                throw new Exception("clip not found");

            fooSource.clip = loadedClip;
            print($"loadedClip, {_filePath}");
        }
        catch (Exception e)
        {
            print("clip not found, fetching");
            StartCoroutine(ElevenLabsVoiceAPI.VoiceGetRequest("Eliran", OnFetchComplete));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            print("playing");
            PlaySource(fooSource.clip);
        }
    }

    private void OnFetchComplete(AudioClip audioClip)
    {
        print("audioClip: " + audioClip);
        audioClip.name = "playerNameAudioClip";
        PlaySource(audioClip);
        AudioClipUtility.SaveAudioClip(audioClip, _filePath);
    }

    private void PlaySource(AudioClip clip)
    {
        fooSource.clip = clip;
        fooSource.time = startTime;
        fooSource.Play();
        Invoke(nameof(StopSource), endTime - startTime);
    }

    private void StopSource()
    {
        fooSource.Stop();
    }

    public void SetPlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        if (titleCard) titleCard.text = $"{playerName.ToUpper().Trim()} IS COLD";
        // StartCoroutine(ElevenLabsVoiceAPI.VoiceGetRequest(playerName));
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