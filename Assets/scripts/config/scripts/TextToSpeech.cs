using System;
using enums;
using UnityEngine;

namespace config.scripts
{
    public class TextToSpeech : MonoBehaviour
    {
        private string _filePath;

        public float ConvertAndWait(string text, PlayerGender gender, Action<AudioClip> onConvertComplete)
        {
            try
            {
                var formattedText = text
                    .Replace(" ", "_")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Replace("?", "");
                _filePath = Application.persistentDataPath + $"/{formattedText}-{gender}.wav";
                var loadedClip = AudioClipUtility.LoadAudioClip(_filePath);

                if (loadedClip == null)
                    throw new Exception("clip not found");

                loadedClip.name = formattedText;
                print($"clip already saved to disk, returning; {_filePath}");
                onConvertComplete(loadedClip);
            }
            catch
            {
                print("clip not found on disk, fetching;");
                StartCoroutine(ElevenLabsVoiceAPI.VoiceGetRequest(text, gender, audioClip =>
                {
                    AudioClipUtility.SaveAudioClip(audioClip, _filePath);
                    onConvertComplete(audioClip);
                }));
            }

            return 0;
        }
    }
}