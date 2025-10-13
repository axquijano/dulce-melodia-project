using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public TutorialStep[] steps;
    public TMP_Text tutorialText;

    [Tooltip("Tiempo extra después de la voz")]
    public float extraDelay = 1.5f;

    private int currentStep = 0;
    private Coroutine autoNextCoroutine;
    private bool stepAdvancedManually;

    void Start()
    {
        ShowStep();
    }

    void ShowStep()
    {
        if (currentStep >= steps.Length)
            return;

        if (autoNextCoroutine != null)
            StopCoroutine(autoNextCoroutine);

        stepAdvancedManually = false;

        foreach (var item in steps)
        {
            if (item.arrowToShow != null)
                item.arrowToShow.SetActive(false);
        }

        TutorialStep step = steps[currentStep];

        tutorialText.text = step.message;

        if (step.arrowToShow != null)
            step.arrowToShow.SetActive(true);

        if (TTSManager.Instance != null)
        {
            TTSManager.Instance.Speak(step.message);
            autoNextCoroutine =
                StartCoroutine(AutoAdvanceAfterTime(step.voiceDuration));
        }
    }

    IEnumerator AutoAdvanceAfterTime(float voiceTime)
    {
        // Esperar duración del mensaje
        yield return new WaitForSeconds(voiceTime);

        // Tiempo extra para comprensión
        yield return new WaitForSeconds(extraDelay);

        if (!stepAdvancedManually)
        {
            NextStep();
        }
    }

    public void NextStep()
    {
        stepAdvancedManually = true;

        if (autoNextCoroutine != null)
            StopCoroutine(autoNextCoroutine);

        currentStep++;
        ShowStep();
    }
}
