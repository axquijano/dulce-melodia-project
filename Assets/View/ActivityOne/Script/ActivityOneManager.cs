using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;


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
    
    void Start()
    {
        ActivityConnector.Instance.StartLevel();
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
        bubbleContainer.GetChild(0).GetComponent<NoteBubble>().SetImagenColor();
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

        for (int i = 1; i < bubbleContainer.childCount; i++)
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

        // ✔ Correcta
        if (pressedNote.noteName == sequence.notes[currentIndex].note.noteName)
        {
            ActivityConnector.Instance.RegisterHit(); 
            PositionFrogAt(currentIndex);
            FeedbackManager.Instance.RegisterHit();
            currentIndex++;

            if (currentIndex >= sequence.notes.Length)
            {
                ActivityConnector.Instance.OnWin(); 
                return;
            }

            HighlightCurrentBubble();
        }
        else
        {
            // ❌ Incorrecta
           ActivityConnector.Instance.RegisterMistake();
            FeedbackManager.Instance.RegisterMistake(); 
            ShowHelpForCurrentKey();
        }

        // ❌ Condición de derrota
        if (ActivityConnector.Instance.Mistakes >= sequence.allowedMistakes)
        {
           ActivityConnector.Instance.OnLose(); 
            return;
        }
    }

}
