using System.Collections;
using UnityEngine;

public class StarsScrambler : MonoBehaviour
{
    [SerializeField] private GameObject[] stars;
    private int _activeStarIndex;
    private float _currentClipLength;

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

            _activeStarIndex++;


            yield return new WaitForSeconds(_currentClipLength);
        }
    }
}