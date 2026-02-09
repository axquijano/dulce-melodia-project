using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;


public class ActivityOneManager : MonoBehaviour
{
    public ActivityDefinition activity;
    public GameObject bubblePrefab;
    public GameObject frogPrefab;
    public Transform bubbleContainer;

    public PianoKey[] pianoKeys;

    private int currentIndex = 1;

    public LevelSequence sequence;
    private Coroutine helpRoutine;
    public HelpConfig config;
    
    private RectTransform frog;
    
    public string childName;

    private int mistakesOnCurrentNote = 0;

    [Header("Mistake Feedback UI")]
    public Image avatarImage;
    public TMP_Text mistakeText;

    [Header("Avatar")]
    public StudentAvatarDatabase avatarDatabase;
    private StudentAvatarData currentAvatar;


    
    void Start()
    {
        ActivityConnector.Instance.StartLevel();
        childName = ProfilesManager.Instance.currentProfile.childName;
        currentAvatar = avatarDatabase.GetById(ProfilesManager.Instance.currentProfile.avatarId);
        avatarImage.gameObject.SetActive(false);
        mistakeText.gameObject.SetActive(false);
        loadSequence();
        GenerateBubbles();
        CreateFrog();      
        PositionFrogAt(0);
        HighlightCurrentBubble();
        LinkPianoKeys();
    }

    void loadSequence()
    {
        activity = GameFlowManager.Instance.selectedActivity;
        sequence = GameFlowManager.Instance.GetCurrentLevelSequence();
        FeedbackManager.Instance.SetMaxMistakes(sequence.allowedMistakes);
        int levelIndex = PlayerPrefs.GetInt("CurrentLevel");

        if (activity == null)
            Debug.LogError("❌ selectedActivity está NULL — no se seleccionó actividad antes de cargar esta escena.");

        if (sequence == null)
            Debug.LogError("❌ sequence está NULL — el nivel no fue seleccionado o el índice es incorrecto.");
    }

    void CreateFrog()
    {
        GameObject frogObj = Instantiate(frogPrefab, bubbleContainer.parent); 
        frog = frogObj.GetComponent<RectTransform>();
    }

    
    void GenerateBubbles()
    {
        foreach (var data in sequence.notes)
        {
            var b = Instantiate(bubblePrefab, bubbleContainer);
            b.GetComponent<NoteBubble>().Setup(data);
        }

        int count = bubbleContainer.childCount;

        if (count > 0)
        {
            bubbleContainer.GetChild(0)
                .GetComponent<NoteBubble>()
                .SetImagenColor();

            bubbleContainer.GetChild(count-1)
                .GetComponent<NoteBubble>()
                .SetImagenColor();
        }
    }


    void PositionFrogAt(int index)
    {
        if (index >= bubbleContainer.childCount) return;

        RectTransform bubble = bubbleContainer.GetChild(index).GetComponent<RectTransform>();

        // Hacer la rana hija de la hoja
        frog.SetParent(bubble);

        // Resetear transform para quedar centrada
        frog.anchorMin = new Vector2(0.5f, 0.5f);
        frog.anchorMax = new Vector2(0.5f, 0.5f);
        frog.pivot = new Vector2(0.5f, 0.5f);

        frog.anchoredPosition = new Vector2(0, 13);  // Centrar justo encima
    }


    void LinkPianoKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    void HighlightCurrentBubble()
    {
        if (helpRoutine != null) StopCoroutine(helpRoutine);

        helpRoutine = StartCoroutine(HelpTimer());

        for (int i = 1; i < bubbleContainer.childCount-1; i++)
        {
            bubbleContainer.GetChild(i)
                .GetComponent<NoteBubble>()
                .Highlight(i, currentIndex);
        }
    }

    IEnumerator HelpTimer()
    {
        yield return new WaitForSeconds(config.delayBeforeHelp);

        if (!config.helpEnabled)
            yield break;

        ShowHelpForCurrentKey();
    }

    void ShowHelpForCurrentKey()
    {
        var targetNote = sequence.notes[currentIndex].note;

        foreach (var key in pianoKeys)
        {
            if (key.noteData.noteName == targetNote.noteName)
            {
                key.ShowHelp();
                break;
            }
        }
    }

    void OnKeyPressed(NoteData pressedNote)
    {       
        if (helpRoutine != null)
            StopCoroutine(helpRoutine);

        avatarImage.gameObject.SetActive(false);
        mistakeText.gameObject.SetActive(false);

        // ✔ Correcta
        if (pressedNote.noteName == sequence.notes[currentIndex].note.noteName)
        {
            mistakesOnCurrentNote = 0; // 🔄 reset

            ActivityConnector.Instance.RegisterHit(); 
            FeedbackManager.Instance.RegisterHit();

            PositionFrogAt(currentIndex);
            currentIndex++;

            // 🎉 Estímulo visual
            var feedback = frog.GetComponentInChildren<VisualFeedback>(true);
            if (feedback != null)
                feedback.ShowNextReward();

            // 🎙️ Voz positiva
            TTSManager.Instance.Speak("Muy bien");

            if (currentIndex >= sequence.notes.Length - 1)
            {
                ActivityConnector.Instance.OnWin(); 
                return;
            }

            HighlightCurrentBubble();
        }
        else
        {
            ActivityConnector.Instance.RegisterMistake();
            FeedbackManager.Instance.RegisterMistake();

            mistakesOnCurrentNote++;

            // Mostrar mensaje + avatar
            avatarImage.gameObject.SetActive(true);
            avatarImage.sprite = currentAvatar.sadSprite;

            mistakeText.gameObject.SetActive(true);
            mistakeText.text = $"{childName}, vamos a intentarlo de nuevo";

            TTSManager.Instance.Speak(
                $"{childName}, vamos a intentarlo de nuevo"
            );

            // 👉 SOLO en el segundo error se da ayuda visual
            if (mistakesOnCurrentNote >= 2)
            {
                ShowHelpForCurrentKey();
            }
        }


        // ❌ Condición de derrota
        if (ActivityConnector.Instance.Mistakes >= sequence.allowedMistakes)
        {
            Debug.Log("❌ Límite de errores alcanzado. Nivel perdido.");
            ActivityConnector.Instance.OnLose(); 
            return;
        }
    }

}
