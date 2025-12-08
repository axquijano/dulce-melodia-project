using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroController : MonoBehaviour
{
    public IntroStepsCollection stepsCollection;

    public TMP_Text stepText;
    public Animator animator;         // Para reproducir una animación si existe

    public GameObject introPanel;
    public GameObject gamePanel;

    public GameObject gameLevelManager;

    public Button nextButton;
    public Button audioButton;
    private int currentStep = 0;

    void Start()
    {
        gamePanel.SetActive(false);
        gameLevelManager.SetActive(false);
        ShowStep(0);
    }

    public void NextStep()
    {
        currentStep++;

        if (currentStep >= stepsCollection.steps.Length)
        {
            EndIntro();
            return;
        }

        ShowStep(currentStep);
    }

    void ShowStep(int index)
    {

        var step = stepsCollection.steps[index];

        stepText.text = step.text;

        if (animator != null && !string.IsNullOrEmpty(step.animationName))
        {
            animator.Play(step.animationName);
        }

                

    }

    public void audioReproducir ()
    {
        var step = stepsCollection.steps[currentStep];
        // AUDIO TTS
        TTSManager.Instance.Speak(step.text);
    }
    void EndIntro()
    {
        introPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameLevelManager.SetActive(true);
    }

    public void OnTTSStepFinished()
    {
        nextButton.interactable = true;
    }

}
