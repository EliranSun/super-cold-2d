using System.Collections;
using UnityEngine;

public class StarsScrambler : MonoBehaviour
{
    [SerializeField] private GameObject[] stars;

    private void Start()
    {
        foreach (var star in stars)
            StartCoroutine(ChangeStarPosition(star));
    }

    private IEnumerator ChangeStarPosition(GameObject star)
    {
        while (true)
        {
            star.SetActive(false);
            star.transform.position = new Vector2(Random.Range(-30, 30), Random.Range(-15, 15));
            star.SetActive(true);

            var clips = star.GetComponent<Animator>().runtimeAnimatorController.animationClips;
            var currentClipLength = 0f;

            foreach (var clip in clips)
            {
                print($"CLIP NAME {clip.name} STAR NAME {star.name}");
                if (clip.name == star.name)
                    currentClipLength = clip.length;
            }

            yield return new WaitForSeconds(currentClipLength);
        }
    }
}