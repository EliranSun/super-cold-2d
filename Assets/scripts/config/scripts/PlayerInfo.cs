using config.scripts;
using enums;
using TMPro;
using UnityEngine;

public enum PlayerPrefsKeys
{
    PlayerName,
    PlayerGender,
    PlayerPartner,
    SeenUniverseDeathSequence
}

public class PlayerInfo : ActionableScript
{
    [SerializeField] private TextMeshProUGUI titleCard;

    private void Start()
    {
        var playerName = GetPlayerName();
        if (titleCard && playerName != "")
            titleCard.text = $"{playerName.ToUpper()} IS COLD";
    }

    public void SetPlayerName(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
            return;

        PlayerPrefs.SetString(PlayerPrefsKeys.PlayerName.ToString(), playerName);
        if (titleCard)
            titleCard.text = $"{playerName.ToUpper().Trim()} IS COLD";
    }

    public void SetPlayerGender(string gender)
    {
        PlayerPrefs.SetString(PlayerPrefsKeys.PlayerGender.ToString(), gender.ToUpper());
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString(PlayerPrefsKeys.PlayerName.ToString());
    }

    public static PlayerGender GetPlayerGender()
    {
        switch (PlayerPrefs.GetString(PlayerPrefsKeys.PlayerGender.ToString()))
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
        PlayerPrefs.SetString(PlayerPrefsKeys.PlayerPartner.ToString(), partner.ToUpper().Trim());
    }

    public static string GetPlayerPartner()
    {
        return PlayerPrefs.GetString(PlayerPrefsKeys.PlayerPartner.ToString());
    }

    public void ResetPlayerGender()
    {
        PlayerPrefs.DeleteKey(PlayerPrefsKeys.PlayerGender.ToString());
    }


    [ContextMenu("Delete All Player Audio Clips")]
    public void DeleteAllPlayerAudioClips()
    {
        AudioClipUtility.DeleteAllAudioClips();
    }
    
    public void SetSeenUniverseDeathSequence()
    {
        PlayerPrefs.SetString(PlayerPrefsKeys.SeenUniverseDeathSequence.ToString(), "true");
    }

    public override void Activate()
    {
        SetSeenUniverseDeathSequence();
    }
    
    public string GetPlayerPrefValue(PlayerPrefsKeys key)
    {
        return PlayerPrefs.GetString(key.ToString());
    }
}