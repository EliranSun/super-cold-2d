using System.Collections;
using action_triggers.scripts;
using UnityEngine;

public class StarsScrambler : MonoBehaviour
{
    [SerializeField] private GameObject[] stars;
    [SerializeField] private bool playAnimationInReverse;
    private int _activeStarIndex;
    private float _currentClipLength;
    private static readonly int Reverse = Animator.StringToHash("Reverse");

    private void Start()
    {
        StartCoroutine(ChangeStarPosition());
    }
    

    private IEnumerator ChangeStarPosition()
    {
        while (true)
        {
            if (_activeStarIndex == stars.Length)
            {
                stars[_activeStarIndex - 1].SetActive(false);
                _activeStarIndex = 0;
            }

            var star = stars[_activeStarIndex];
            var clips = star.GetComponent<Animator>().runtimeAnimatorController.animationClips;


            clips[0].name = star.name;
            _currentClipLength = clips[0].length;

            if (_activeStarIndex > 0)
                stars[_activeStarIndex - 1].SetActive(false);

            star.SetActive(true);

            if (playAnimationInReverse)
            {
                star.GetComponent<Animator>().SetBool(Reverse, true);
            }

            _activeStarIndex++;


            yield return new WaitForSeconds(_currentClipLength);
        }
    }
    
    public void ReverseAnimation()
    {
        playAnimationInReverse = true;
    }
    
    public void OnNotify(DialogueAction message)
    {
        if (message == DialogueAction.ReverseAnimation)
        {
            print("Reverse animation");
            ReverseAnimation();
        }
    }
}