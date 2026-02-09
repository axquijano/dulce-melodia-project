using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Sequence")]
    public LevelSequence sequence;
    public NoteBubble[] bubbles;

    [Header("Piano")]
    public PianoKey[] pianoKeys;

    [Header("Frog")]
    public RectTransform frog;
    public Animator frogAnimator;
    private VisualFeedback feedback;

    [Header("Tutorial UI")]
    public TutorialStep[] steps;
    public TMP_Text tutorialText;
    public float extraDelay = 1.5f;

    private int currentStep = 0;
    private int currentIndex = 1;

    private Coroutine autoNextCoroutine;
    private bool stepAdvancedManually;
    private bool tutorialGameplayActive = false;

    // Control del error forzado
    private bool forcedMistakeDone = false;
    private string forcedWrongNote = "D";

    [Header("Avatar")]
    public StudentAvatarDatabase avatarDatabase;
    public Image avatarImage; // UI Image del muñeco

    private StudentAvatarData currentAvatar;
    private string childName;


    #region UNITY

    void Start()
    {
        // Perfil actual
        var profile = ProfilesManager.Instance.currentProfile;
        childName = profile.childName;

        // Obtener avatar del niño
        currentAvatar = avatarDatabase.GetById(profile.avatarId);

        // Mostrar avatar base
        if (currentAvatar != null && avatarImage != null)
            avatarImage.sprite = currentAvatar.avatarSprite;

        feedback = frog.GetComponentInChildren<VisualFeedback>(true);
        if (feedback == null)
        {
            Debug.LogError("No se encontró VisualFeedback dentro del prefab de la rana");
        }

        InitializeBubbles();
        FeedbackManager.Instance.SetMaxMistakes(sequence.allowedMistakes);
        DisableAllKeys();
        HighlightCurrentBubble();
        StartCoroutine(IntroGreeting());
    }

    IEnumerator IntroGreeting()
    {
        // Mostrar texto de saludo
        tutorialText.text = $"Hola {childName}, me alegra verte. Vamos a aprender música juntos ";

        // Avatar feliz
        if (currentAvatar != null && avatarImage != null)
            avatarImage.sprite = currentAvatar.happySprite;
        TTSManager.Instance.Speak(
            $"Hola {childName}, me alegra verte. Vamos a aprender música juntos"
        );
        yield return new WaitForSeconds(3.5f);
        tutorialText.text = "";

        //Comenzar con el tutorial
        ShowStep();
    }



    #endregion

    #region INTRO STEPS

    void ShowStep()
    {
        if (currentStep >= steps.Length)
            return;

        if (autoNextCoroutine != null)
            StopCoroutine(autoNextCoroutine);

        stepAdvancedManually = false;

        foreach (var s in steps)
            if (s.arrowToShow != null)
                s.SetArrowsActive(false);

        var step = steps[currentStep];
        tutorialText.text = step.message;

        if (step.arrowToShow != null)
            step.SetArrowsActive(true);

        TTSManager.Instance.Speak(step.message);
        autoNextCoroutine = StartCoroutine(AutoAdvance(step.voiceDuration));
    }

    IEnumerator AutoAdvance(float voiceDuration)
    {
        yield return new WaitForSeconds(voiceDuration + extraDelay);

        if (!stepAdvancedManually)
            NextStep();
    }

    public void NextStep()
    {
        stepAdvancedManually = true;

        if (autoNextCoroutine != null)
            StopCoroutine(autoNextCoroutine);

        currentStep++;

        if (currentStep >= steps.Length)
        {
            HideTutorialUI();
            StartTutorialGameplay();
            return;
        }

        ShowStep();
    }

    void HideTutorialUI()
    {
        tutorialText.text = "";
        avatarImage.gameObject.SetActive(false);

        foreach (var step in steps)
            if (step.arrowToShow != null)
                step.SetArrowsActive(false);
    }

    #endregion

    #region GAMEPLAY TUTORIAL

    void StartTutorialGameplay()
    {
        tutorialGameplayActive = true;

        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;

        HighlightCurrentBubble();
        ActivateCorrectKey();

        TTSManager.Instance.Speak("Toca la nota correcta para que René avance");
    }

    void OnKeyPressed(NoteData pressedNote)
    {
        if (!tutorialGameplayActive)
            return;

        string correctNote =
            sequence.notes[currentIndex].note.noteName;

        if (pressedNote.noteName == correctNote)
        {
            HandleCorrect();
        }
        else
        {
            HandleMistake();
        }
    }

    #endregion

    #region GAMEPLAY LOGIC

    void HandleCorrect()
    {
        FeedbackManager.Instance.RegisterHit();

        MoveFrogTo(currentIndex);
        bubbles[currentIndex].SetImagenColor();

        currentIndex++;

        TTSManager.Instance.Speak(
                "Muy bien"
            );
        feedback.ShowNextReward();

        if (currentIndex >= sequence.notes.Length- 1)
        {
            StartCoroutine(TutorialEnding());
            return;
        }

        HighlightCurrentBubble();
        DisableAllKeys();

        // 🔴 ERROR FORZADO EN LA TERCERA HOJA
        if (currentIndex == 2 && !forcedMistakeDone)
        {
            Debug.Log("currentIndex == 2 && !forcedMistakeDone "+ currentIndex + " "+forcedMistakeDone );
            forcedMistakeDone = true;
            ActivateOnly(forcedWrongNote);

            TTSManager.Instance.Speak(
                "¿Qué pasará si tocamos otra nota?"
            );
        }
        else
        {
            ActivateCorrectKey();
        }
    }

    void HandleMistake()
    {
       
        FeedbackManager.Instance.RegisterMistake();
        
        TTSManager.Instance.Speak("Ups… René perdió un poquito de energía. ¡Intenta de nuevo!");

        DisableAllKeys();
        ActivateCorrectKey();
 
    }

    #endregion

    #region VISUALS

    void InitializeBubbles()
    {
        int count = Mathf.Min(bubbles.Length, sequence.notes.Length);

        for (int i = 0; i < count-1; i++)
        {
            bubbles[i].Setup(sequence.notes[i]);
        }

        bubbles[0].SetImagenColor();
    }

    void HighlightCurrentBubble()
    {
        Debug.Log("Current index en highlight "+bubbles.Length);
        for (int i = 1; i < bubbles.Length-1; i++)
        {
             Debug.Log("Current index en highlight "+i);
              Debug.Log("Current index en highlight "+currentIndex);
            bubbles[i].Highlight(i, currentIndex);
        }
    }

    void MoveFrogTo(int index)
    {
        RectTransform target =
            bubbles[index].GetComponent<RectTransform>();

        frog.SetParent(target);
        frog.anchoredPosition = new Vector2(0, 13);
    }

    #endregion

    #region PIANO KEYS

    void ActivateCorrectKey()
    {
        string correctNote =
            sequence.notes[currentIndex].note.noteName;

        ActivateOnly(correctNote);
    }

    void ActivateOnly(string noteName)
    {
        foreach (var key in pianoKeys)
            key.SetKeyEnabled(key.noteData.noteName == noteName);
    }

    void DisableAllKeys()
    {
        foreach (var key in pianoKeys)
            key.SetKeyEnabled(false);
    }

    #endregion

    public void FinishTutorial()
    {
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");

        ActivityEntry activity =
            ProfilesManager.Instance.currentProfile.activities[activityIndex];

        activity.value.tutorialSeen = true;
        ProfilesManager.Instance.SaveProfiles();

        SceneLoader.Instance.LoadScene(
            GameFlowManager.Instance.selectedActivity.gameplaySceneName
        );
    }

    IEnumerator TutorialEnding()
    {
        tutorialGameplayActive = false;

        // Desactivar teclas
        DisableAllKeys();

        // Mostrar avatar celebrando
        if (currentAvatar != null && avatarImage != null)
        {
            avatarImage.gameObject.SetActive(true);
            avatarImage.sprite = currentAvatar.celebrationSprite;
        }

        // Texto de cierre
        tutorialText.text = $"¡Muy bien {childName}! Ayudaste a René a llegar a su comida";

        // Voz
        TTSManager.Instance.Speak(
            $"¡Muy bien {childName}! Ayudaste a René a llegar a su comida"
        );

        // Pequeña pausa para asimilar el logro
        yield return new WaitForSeconds(3.5f);

        FinishTutorial();
    }


}
