using System.IO;
using UnityEngine;

namespace config.scripts
{
    public static class AudioClipUtility
    {
        public static void SaveAudioClip(AudioClip clip, string path)
        {
            // Convert AudioClip to a WAV byte array
            var audioData = WavUtility.FromAudioClip(clip);

            // Write byte array to file
            File.WriteAllBytes(path, audioData);
        }

        public static AudioClip LoadAudioClip(string path)
        {
            // Check if the file exists
            if (!File.Exists(path))
            {
                Debug.LogError("File not found at " + path);
                return null;
            }

            // Read byte array from file
            var audioData = File.ReadAllBytes(path);

            // Convert byte array to AudioClip
            return WavUtility.ToAudioClip(audioData);
        }
    }
}