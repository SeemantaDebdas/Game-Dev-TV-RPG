using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    //--------------------------------------Formula for fading with respect to seconds-----------------------------------------
    //number of frames = time.deltaTime/time taken to fade
    //deltaFadeValue = 1/number of frames
    //deltaFadeValue = 1 * (time.deltaTime/time taken to fade)

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeIn(float time)
    {
        while(canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }
}
