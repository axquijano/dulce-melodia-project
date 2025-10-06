using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialActivityOne : MonoBehaviour
{
    
    [Header("Prefabs")]
    public GameObject bubblePrefab;
    public GameObject frogPrefab;
    public GameObject flyPrefab;

    public Transform bubbleContainer;
    public PianoKey[] pianoKeys;

    public int currentIndex = 1;

    public LevelSequence sequence;
    private RectTransform frog;

    [Header("Tutorial Settings")]
    public bool showIntroMessage = true;



    void Start()
    {
       
        GenerateBubbles();
        CreateFrog();
        PositionFrogAt(0);

        SetupTutorialSequence();
        HighlightCurrentBubble();
        LinkPianoKeys();

        if (showIntroMessage)
            ShowIntro();
    }

    // ---------------------------------------------------------
    //  Crear rana y burbujas
    // ---------------------------------------------------------
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
        Instantiate(flyPrefab, bubbleContainer);
        // Primera hoja en color especial
        bubbleContainer.GetChild(0).GetComponent<NoteBubble>().SetImagenColor();

    }

    void PositionFrogAt(int index)
    {
        if (index >= bubbleContainer.childCount) return;

        RectTransform bubble = bubbleContainer.GetChild(index).GetComponent<RectTransform>();

        frog.SetParent(bubble);

        frog.anchorMin = new Vector2(0.5f, 0.5f);
        frog.anchorMax = new Vector2(0.5f, 0.5f);
        frog.pivot = new Vector2(0.5f, 0.5f);

        frog.anchoredPosition = new Vector2(0, 13);
    }

    // ---------------------------------------------------------------------------
    //  INICIO DEL TUTORIAL
    // ---------------------------------------------------------------------------
    void ShowIntro()
    {
        Debug.Log("⭐ Bienvenido al tutorial — Toca la nota correcta para que René salte.");
    }

    void SetupTutorialSequence()
    {


        // Bloquea todas las teclas
        foreach (var key in pianoKeys)
            key.SetKeyEnabled(false);
    }

    void LinkPianoKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    // ---------------------------------------------------------------------------
    //  RESALTAR HOJA + ACTIVAR SOLO LA TECLA CORRECTA
    // ---------------------------------------------------------------------------
    void HighlightCurrentBubble()
    {
        
        var targetNote = sequence.notes[currentIndex].note;

        for (int i = 1; i < bubbleContainer.childCount-1; i++)
        {
            bubbleContainer.GetChild(i)
                .GetComponent<NoteBubble>()
                .Highlight(i, currentIndex);
        }

        // Activar SOLO la tecla correcta
        foreach (var key in pianoKeys)
        {
            bool isCorrect = key.noteData.noteName == targetNote.noteName;
            key.SetKeyEnabled(isCorrect);

            if (isCorrect)
                key.ShowHelp(); // Brillo solo en tutorial
            else
                key.ResetVisualHelp();
        }
        
    }

    // ---------------------------------------------------------------------------
    //     AL TOCAR UNA TECLA
    // ---------------------------------------------------------------------------
    void OnKeyPressed(NoteData pressedNote)
    {
        Debug.Log($"🎹 Tecla presionada: {pressedNote.noteName} con indice {currentIndex}");
       
        // Evita tocar si ya está fuera del rango
        if (currentIndex >= sequence.notes.Length)
            return;

        if (pressedNote.noteName != sequence.notes[currentIndex].note.noteName)
            return;

        
        StartCoroutine(JumpToNextStep());
        
    }


    IEnumerator JumpToNextStep()
    {
        PositionFrogAt(currentIndex);
        currentIndex++;

        // FIN DEL TUTORIAL
        if (currentIndex >= sequence.notes.Length)
        {
            EndTutorial();
            yield break;
        }
        
        HighlightCurrentBubble();

    }

    // ---------------------------------------------------------------------------
    //    FINAL DEL TUTORIAL
    // ---------------------------------------------------------------------------
    void EndTutorial()
    {
        Debug.Log("🎉 Tutorial completado, ¡René llegó a su comida!");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Activity1_Main"); 
    }
}
