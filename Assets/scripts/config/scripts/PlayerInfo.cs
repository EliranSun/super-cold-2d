using enums;
using TMPro;
using UnityEngine;

public class PlayerInfo : ObserverSubject
{
    [SerializeField] private TextMeshProUGUI titleCard;

    public void SetPlayerName(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
            return;

        PlayerPrefs.SetString("PlayerName", playerName);
        if (titleCard)
            titleCard.text = $"{playerName.ToUpper().Trim()} IS COLD";
    }

    public void SetPlayerGender(string gender)
    {
        PlayerPrefs.SetString("PlayerGender", gender.ToUpper());
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName");
    }

    public static PlayerGender GetPlayerGender()
    {
        switch (PlayerPrefs.GetString("PlayerGender"))
        {
            case "MALE":
                return PlayerGender.Male;

            case "FEMALE":
                return PlayerGender.Female;

            default:
                return PlayerGender.None;
        }
    }
}