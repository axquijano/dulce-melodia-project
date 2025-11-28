using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialActivityTwoManager : MonoBehaviour
{
    [Header("Tutorial")]
    public TutorialStep[] steps;
    public TMP_Text tutorialText;
    public float extraDelay = 1.5f;

    private int currentStep = 0;
    private bool waitingForAction = false;

    [Header("Avatar")]
    public StudentAvatarDatabase avatarDatabase;
    public Image avatarImage;

    [Header("Demo Balloon")]
    public TutorialBalloonDemo demoBalloon;
    public PianoKey[] pianoKeys;

    [Header("Energy Arrow")]
    public GameObject energyArrow;

    private StudentAvatarData avatar;
    private string childName;

    void Start()
    {
        var profile = ProfilesManager.Instance.currentProfile;
        childName = profile.childName;
        avatar = avatarDatabase.GetById(profile.avatarId);

        if (avatar != null && avatarImage != null)
            avatarImage.sprite = avatar.happySprite;

        foreach (var key in pianoKeys)
        {
            key.onKeyPressed += OnKeyPressed;
            key.SetKeyEnabled(false);
        }

        FeedbackManager.Instance.SetMaxMistakes(3);

        if (energyArrow != null)
            energyArrow.SetActive(false);

        StartCoroutine(IntroGreeting());
    }

    IEnumerator IntroGreeting()
    {
        tutorialText.text = $"Hola {childName}, vamos a jugar con los globos musicales";
        TTSManager.Instance.Speak(tutorialText.text);
        yield return new WaitForSeconds(5f);

        ShowStep();
    }

    void ShowStep()
    {
        if (currentStep >= steps.Length)
        {
            EndTutorial();
            return;
        }

        waitingForAction = false;

        foreach (var s in steps)
            s.SetArrowsActive(false);

        var step = steps[currentStep];

        tutorialText.text = step.message;
        step.SetArrowsActive(true);

        TTSManager.Instance.Speak(step.message);
        DisableAllKeys();

        if (step.waitForAction)
            StartCoroutine(WaitThenEnableAction(step.voiceDuration));
        else
            StartCoroutine(AutoNext(step.voiceDuration));
    }

    IEnumerator AutoNext(float voiceDuration)
    {
        yield return new WaitForSeconds(voiceDuration + extraDelay);
        NextStep();
    }

    IEnumerator WaitThenEnableAction(float voiceDuration)
    {
        yield return new WaitForSeconds(voiceDuration + extraDelay);
        waitingForAction = true;
        EnableOnlyExpectedKey();
    }

    void NextStep()
    {
        currentStep++;
        ShowStep();
    }

    void OnKeyPressed(NoteData note)
    {
        if (!waitingForAction) return;

        bool exploded = demoBalloon.TryExplode(note);
        if (!exploded) return;

        waitingForAction = false;
        DisableAllKeys();
        HideAllArrows();

        StartCoroutine(AfterCorrect());
    }

    IEnumerator AfterCorrect()
    {
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(ShowMissExample());

        currentStep++;
        ShowStep();
    }

    IEnumerator ShowMissExample()
    {
        demoBalloon.ResetBalloon(demoBalloon.expectedNote);
        demoBalloon.transform.localPosition = new Vector3(0, -250f, 0);

        tutorialText.text = "Ahora mira este globo";
        TTSManager.Instance.Speak(tutorialText.text);
        yield return new WaitForSeconds(3f);

        tutorialText.text = "Observa lo que pasa si no lo tocamos";
        TTSManager.Instance.Speak(tutorialText.text);
        yield return new WaitForSeconds(3f);

        bool missed = false;
        demoBalloon.OnMissed += () => missed = true;

        demoBalloon.StartMoving();

        yield return new WaitUntil(() => missed);

        tutorialText.text = "El globo se fue";
        TTSManager.Instance.Speak(tutorialText.text);
        yield return new WaitForSeconds(2.5f);

        FeedbackManager.Instance.RegisterMistake();

        if (energyArrow != null)
            energyArrow.SetActive(true);

        tutorialText.text = "Cuando no tocamos el globo, nuestra energía baja";
        TTSManager.Instance.Speak(tutorialText.text);
        yield return new WaitForSeconds(4f);

        if (energyArrow != null)
            energyArrow.SetActive(false);
    }

    void DisableAllKeys()
    {
        foreach (var key in pianoKeys)
        {
            key.SetKeyEnabled(false);
            key.ResetVisualHelp();
        }
    }

    void EnableOnlyExpectedKey()
    {
        foreach (var key in pianoKeys)
        {
            bool correct = key.noteData == demoBalloon.expectedNote;
            key.SetKeyEnabled(correct);

            if (correct)
                key.ShowHelp();
        }
    }

    void HideAllArrows()
    {
        foreach (var s in steps)
            s.SetArrowsActive(false);
    }

    void EndTutorial()
    {
        tutorialText.text = $"¡Muy bien {childName}! Ahora sí vamos a jugar";

        if (avatar != null && avatarImage != null)
            avatarImage.sprite = avatar.celebrationSprite;

        TTSManager.Instance.Speak(tutorialText.text);
        MarkTutorialAsSeen();

        StartCoroutine(GoToGame());
    }

    void MarkTutorialAsSeen()
    {
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");

        ActivityEntry activity =
            ProfilesManager.Instance.currentProfile.activities[activityIndex];

        activity.value.tutorialSeen = true;
        ProfilesManager.Instance.SaveProfiles();
    }

    IEnumerator GoToGame()
    {
        yield return new WaitForSeconds(3f);

        SceneLoader.Instance.LoadScene(
            GameFlowManager.Instance.selectedActivity.gameplaySceneName
        );
    }
}
