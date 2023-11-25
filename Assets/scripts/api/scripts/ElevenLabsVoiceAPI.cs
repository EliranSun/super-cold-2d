using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ElevenLabsVoiceAPI : MonoBehaviour
{
    private const string RachelVoiceId = "21m00Tcm4TlvDq8ikWAM";
    private const string AdamVoiceId = "pNInz6obpgDQGcFmaJgB";
    private static bool _isVoiceRequestSent;
    public static AudioClip PlayerNameAudioClip { get; private set; }

    public static IEnumerator VoiceGetRequest(string playerName, Action<AudioClip> callback)
    {
        if (_isVoiceRequestSent)
        {
            print("VoiceGetRequest() already sent");
            yield break;
        }


        var uri = $"https://api.elevenlabs.io/v1/text-to-speech/{AdamVoiceId}";
        var webRequest = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG);
        webRequest.SetRequestHeader("xi-api-key", "3fa9af49ce49fb0e324cce37f59ae4f2");

        var jsonBody = $@"
        {{
            ""model_id"": ""eleven_monolingual_v1"",
            ""text"": ""Poor {char.ToUpper(playerName[0]) + playerName.Substring(1)}... she is so cold."",
            ""voice_settings"": {{
                ""stability"": 0.71,
                ""similarity_boost"": 0.52,
                ""style"": 0,
                ""use_speaker_boost"": true
            }}
        }}";
        Debug.Log(jsonBody);
        var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        // Create UnityWebRequest
        var www = new UnityWebRequest(uri, "POST");
        print("www: " + www);
        www.SetRequestHeader("xi-api-key", "3fa9af49ce49fb0e324cce37f59ae4f2");

        // Set DownloadHandler to handle the response as AudioClip
        www.downloadHandler = new DownloadHandlerAudioClip(uri, AudioType.MPEG);

        // Set UploadHandler to send the JSON body
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.uploadHandler.contentType = "application/json";

        print("Sending request...");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            print(www.error);
        }
        else
        {
            var audioClip = DownloadHandlerAudioClip.GetContent(www);
            PlayerNameAudioClip = audioClip;
            print("PlayerNameAudioClip: " + PlayerNameAudioClip);
            callback?.Invoke(audioClip);
        }

        _isVoiceRequestSent = true;
    }
}