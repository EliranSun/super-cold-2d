using System.IO;
using UnityEngine;

namespace config.scripts
{
    public static class AudioClipUtility
    {
        public static void SaveAudioClip(AudioClip clip, string path)
        {
            var audioData = WavUtility.FromAudioClip(clip);
            File.WriteAllBytes(path, audioData);
        }

        public static void DeleteAllAudioClips()
        {
            var files = Directory.GetFiles(Application.persistentDataPath, "*.wav");
            foreach (var file in files) File.Delete(file);
        }

        public static AudioClip LoadAudioClip(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("File not found at " + path);
                return null;
            }

            var audioData = File.ReadAllBytes(path);
            return WavUtility.ToAudioClip(audioData);
        }
    }
}