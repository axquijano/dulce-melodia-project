using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;


public class ActivityOneManager : MonoBehaviour
{
    public LevelSequence sequence;
    public GameObject bubblePrefab;
    public Transform bubbleContainer;
    public PianoKey[] pianoKeys;

    private int currentIndex = 0;

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
        if (helpRoutine != null) StopCoroutine(helpRoutine);

        helpRoutine = StartCoroutine(HelpTimer());

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
        if (helpRoutine != null)
            StopCoroutine(helpRoutine);

        // ✔ Correcta
        if (pressedNote.noteName == sequence.notes[currentIndex].noteName)
        {
            ActivityConnector.Instance.RegisterHit();
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
        if (ActivityConnector.Instance.mistakes >= sequence.notes.Length)
        {
            ActivityConnector.Instance.OnLose();
            return;
        }
    }
}
