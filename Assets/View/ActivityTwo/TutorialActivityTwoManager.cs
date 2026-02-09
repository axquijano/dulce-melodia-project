using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialActivityTwoManager : MonoBehaviour
{
    [Header("Tutorial")]
    public TutorialStep[] steps;
    public TMP_Text tutorialText;

    private int currentStep = 0;
    private bool waitingForAction = false;

    [Header("Avatar")]
    public StudentAvatarDatabase avatarDatabase;
    public Image avatarImage;

    [Header("Demo")]
    public TutorialBalloonDemo demoBalloon;
    public PianoKey[] pianoKeys;

    private StudentAvatarData avatar;
    private string childName;

    void Start()
    {
        var profile = ProfilesManager.Instance.currentProfile;
        childName = profile.childName;
        avatar = avatarDatabase.GetById(profile.avatarId);

        avatarImage.sprite = avatar.happySprite;

        foreach (var key in pianoKeys)
        {
            key.onKeyPressed += OnKeyPressed;
            key.SetKeyEnabled(false); // 🔒 bloquear todo
        }

        ShowStep();
    }

    // -------------------------
    // FLUJO DEL TUTORIAL
    // -------------------------

    void ShowStep()
    {
        if (currentStep >= steps.Length)
        {
            EndTutorial();
            return;
        }

        // Limpiar flechas
        foreach (var s in steps)
            s.SetArrowsActive(false);

        var step = steps[currentStep];

        tutorialText.text = step.message;
        step.SetArrowsActive(true);

        TTSManager.Instance.Speak(step.message);

        DisableAllKeys();

        if (step.waitForAction)
        {
            waitingForAction = true;
            EnableOnlyExpectedKey();
        }
        else
        {
            StartCoroutine(AutoNext(step.voiceDuration));
        }
    }

    IEnumerator AutoNext(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextStep();
    }

    void NextStep()
    {
        waitingForAction = false;
        currentStep++;
        ShowStep();
    }

    // -------------------------
    // INPUT
    // -------------------------

    void OnKeyPressed(NoteData note)
    {
        if (!waitingForAction) return;

        bool exploded = demoBalloon.TryExplode(note);

        if (!exploded) return;

        NextStep();
    }

    // -------------------------
    // CONTROL DE TECLAS
    // -------------------------

    void DisableAllKeys()
    {
        foreach (var key in pianoKeys)
            key.SetKeyEnabled(false);
    }

    void EnableOnlyExpectedKey()
    {
        foreach (var key in pianoKeys)
            key.SetKeyEnabled(key.noteData == demoBalloon.expectedNote);
    }


    // -------------------------
    // FIN DEL TUTORIAL
    // -------------------------

    void EndTutorial()
    {
        tutorialText.text = $"¡Muy bien {childName}! Ahora vamos a jugar 🎈";
        avatarImage.sprite = avatar.celebrationSprite;

        TTSManager.Instance.Speak(tutorialText.text);

        StartCoroutine(GoToGame());
    }

    IEnumerator GoToGame()
    {
        yield return new WaitForSeconds(2.5f);
        SceneLoader.Instance.LoadScene(
            GameFlowManager.Instance.selectedActivity.gameplaySceneName
        );
    }
}
