using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;

public class NoteSequenceManager : MonoBehaviour
{
    public LevelSequence sequence;              // arrástralo desde el inspector
    public GameObject bubblePrefab;             // prefab de la bolita
    public Transform bubbleContainer;           // un horizontal layout
    public PianoKey[] pianoKeys;                // todas las teclas del piano

    private int currentIndex = 0;               // nota actual


    private Coroutine helpRoutine;
    public HelpConfig config;
    
    void Start()
    {
        GenerateBubbles();
        HighlightCurrentBubble();
        LinkPianoKeys();
    }

    void GenerateBubbles()
    {
        foreach (var note in sequence.notes)
        {
            var b = Instantiate(bubblePrefab, bubbleContainer);
            b.GetComponent<NoteBubble>().Setup(note);
        }
    }

    void LinkPianoKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    void HighlightCurrentBubble()
    {
        // cancelar ayuda anterior
        if (helpRoutine != null) StopCoroutine(helpRoutine);

        // iniciar el temporizador de ayuda
        helpRoutine = StartCoroutine(HelpTimer());

        // highlight visual de las burbujas
        for (int i = 0; i < bubbleContainer.childCount; i++)
        {
            bubbleContainer.GetChild(i)
                .GetComponent<NoteBubble>()
                .Highlight(i == currentIndex);
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
        var targetNote = sequence.notes[currentIndex];

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
        // cancelar ayuda
        if (helpRoutine != null)
            StopCoroutine(helpRoutine);


        // nota correcta
        if (pressedNote.noteName == sequence.notes[currentIndex].noteName)
        {
            FeedbackManager.Instance.RegisterHit();
            currentIndex++;

            if (currentIndex >= sequence.notes.Length)
            {
                GameUIManager.Instance.ShowWin();
                Debug.Log("Nivel completado!");
                return;
            }

            HighlightCurrentBubble();
        }
        else
        {
            FeedbackManager.Instance.RegisterMistake();
            ShowHelpForCurrentKey();
            Debug.LogWarning("Nota incorrecta");
        }

        if (FeedbackManager.Instance.getMistakes() >= sequence.notes.Length)
            {
                GameUIManager.Instance.ShowLose();
                Debug.Log("Nivel perdido!");
                return;
            }
    }


}
