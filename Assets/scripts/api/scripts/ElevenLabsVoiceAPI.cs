using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ElevenLabsVoiceAPI : MonoBehaviour
{
    private static bool _isVoiceRequestSent;
    public static AudioClip PlayerNameAudioClip { get; private set; }

    public static IEnumerator VoiceGetRequest(string playerName)
    {
        if (_isVoiceRequestSent)
            yield break;

        var uri = "https://api.elevenlabs.io/v1/text-to-speech/21m00Tcm4TlvDq8ikWAM";
        var webRequest = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG);
        webRequest.SetRequestHeader("xi-api-key", "3fa9af49ce49fb0e324cce37f59ae4f2");

        var jsonBody = $@"
        {{
            ""text"": ""{playerName}"",
            ""model_id"": ""eleven_monolingual_v1"",
            ""voice_settings"": {{
                ""stability"": 0,
                ""similarity_boost"": 0,
                ""style"": 0,
                ""use_speaker_boost"": true
            }}
        }}";
        var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        // Create UnityWebRequest
        var www = new UnityWebRequest(uri, "POST");
        www.SetRequestHeader("xi-api-key", "3fa9af49ce49fb0e324cce37f59ae4f2");

        // Set DownloadHandler to handle the response as AudioClip
        www.downloadHandler = new DownloadHandlerAudioClip(uri, AudioType.MPEG);

        // Set UploadHandler to send the JSON body
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.uploadHandler.contentType = "application/json";

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            var audioClip = DownloadHandlerAudioClip.GetContent(www);
            PlayerNameAudioClip = audioClip;
        }

        _isVoiceRequestSent = true;
    }
}