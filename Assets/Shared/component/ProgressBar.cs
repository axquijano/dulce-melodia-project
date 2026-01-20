using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour 
{
    [SerializeField]
    private Image progressBarImage;
    private float maxValue;

    public void SetMaxMistakes ( float value){
        maxValue = value;
    }

    public void UpdateProgressBar(float progress){
        if (maxValue <= 0) return;
        float targetProgress = 1f - (progress / maxValue);
        targetProgress = Mathf.Clamp01(targetProgress);
        float previousProgress = progressBarImage.fillAmount;
        StartCoroutine(ProgressBarAnimation(targetProgress, previousProgress));

    }

    IEnumerator ProgressBarAnimation (float targetProgress, float previousProgress){
        float transitionTime = 0.5f , elapsedTime = 0f;
        while( elapsedTime < transitionTime){
            elapsedTime += Time.deltaTime;
            progressBarImage.fillAmount = Mathf.Lerp(previousProgress, targetProgress, elapsedTime / transitionTime);
            yield return null;
        }
        progressBarImage.fillAmount = targetProgress;
    }

}