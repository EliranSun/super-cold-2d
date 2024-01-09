using System;
using enums;
using UnityEngine;

namespace config.scripts
{
    public class TextToSpeech : MonoBehaviour
    {
        private string _filePath;

        private void Start()
        {
            print(Application.persistentDataPath);
        }

        public float ConvertText(string text, PlayerGender gender, Action<AudioClip> onConvertComplete)
        {
            try
            {
                var formattedText = text
                    .Replace(" ", "_")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Replace("?", "");
                _filePath = Application.persistentDataPath + $"/{formattedText.Substring(0, 20)}-{gender}.wav";
                var loadedClip = AudioClipUtility.LoadAudioClip(_filePath);

                if (loadedClip == null)
                    throw new Exception("clip not found");

                loadedClip.name = formattedText;
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