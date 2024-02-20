using enums;
using UnityEngine;

public class CharacterGenderHandler : MonoBehaviour
{
    [SerializeField] private bool isPlayer = true;
    private Animator _animator;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();

        if (isPlayer)
        {
            SetPlayerAnimator();
            SetPlayerVoice();
        } 
        else
        {
            SetPartnerAnimator();
            SetPartnerVoice();
        }
    }

    void SetPlayerAnimator()
    {
        // TODO: Replace character sprite and animations
        // if (PlayerPreferences.GetPlayerGender() == PlayerGender.Female)
        // if (PlayerPreferences.GetPlayerGender() == PlayerGender.Male)
    }

    void SetPartnerAnimator()
    {
        // TODO: Replace character sprite and animations
        // if (PlayerPreferences.GetPlayerGender() == PlayerGender.Female)
        // if (PlayerPreferences.GetPlayerGender() == PlayerGender.Male)
    }

    void SetPlayerVoice()
    {
        // TODO: for end sequence
    }

    void SetPartnerVoice()
    {
        // TODO: for end sequence
    }
}
