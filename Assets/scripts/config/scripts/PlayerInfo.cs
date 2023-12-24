using enums;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleCard;

    private void Start()
    {
        var playerName = GetPlayerName();
        if (titleCard && playerName != "") titleCard.text = $"{playerName.ToUpper()} IS COLD";
    }

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

    public static void SetPlayerPartner(string partner)
    {
        PlayerPrefs.SetString("PlayerPartner", partner.ToUpper().Trim());
    }

    public static string GetPlayerPartner()
    {
        return PlayerPrefs.GetString("PlayerPartner");
    }
}