using System.Collections;
using UnityEngine;

public class FadeGameObjectsSpriteRenderer : ActionableScript
{
    [SerializeField] private float transitionTimeInSeconds = 5;
    [SerializeField] private SpriteRenderer currentBackgroundSpriteRenderer;
    [SerializeField] private SpriteRenderer newBackgroundSpriteRenderer;

    public override void Activate()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (newBackgroundSpriteRenderer.color.a < 1)
        {
            var alphaChangePerFrame = 1f / (transitionTimeInSeconds / Time.deltaTime);

            print(alphaChangePerFrame);

            currentBackgroundSpriteRenderer.color -= new Color(0, 0, 0, alphaChangePerFrame);
            newBackgroundSpriteRenderer.color += new Color(0, 0, 0, alphaChangePerFrame);

            yield return new WaitForEndOfFrame();
        }
    }
}