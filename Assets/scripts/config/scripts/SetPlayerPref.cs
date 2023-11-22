using TMPro;
using UnityEngine;

public class SetPlayerPref : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleCard;

    public void SetPlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        if (titleCard) titleCard.text = $"{playerName.ToUpper().Trim()} IS COLD";
        StartCoroutine(ElevenLabsVoiceAPI.VoiceGetRequest(playerName));
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