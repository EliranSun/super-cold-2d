using System.Collections;
using System.Collections.Generic;
using enums;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterGenderHandler : MonoBehaviour
{
    [SerializeField] private bool isPlayer = true;
    [SerializeField] AnimatorController maleAnimator;
    [SerializeField] AnimatorController femaleAnimator;
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
        if (PlayerPreferences.GetPlayerGender() == PlayerGender.Female)
            _animator.runtimeAnimatorController = femaleAnimator;
        
        if (PlayerPreferences.GetPlayerGender() == PlayerGender.Male)
            _animator.runtimeAnimatorController = maleAnimator;
    }

    void SetPartnerAnimator()
    {
        if (PlayerPreferences.GetPlayerGender() == PlayerGender.Female)
            _animator.runtimeAnimatorController = maleAnimator;
        
        if (PlayerPreferences.GetPlayerGender() == PlayerGender.Male)
            _animator.runtimeAnimatorController = femaleAnimator;
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
