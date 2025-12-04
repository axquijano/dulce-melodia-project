/* using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialActivityOne : MonoBehaviour
{
    
    public GameObject bubblePrefab;
    public GameObject frogPrefab;
    public Transform bubbleContainer;
    public PianoKey[] pianoKeys;

    private int currentIndex = 1;

    public LevelSequence sequence;
    private RectTransform frog;

    [Header("Tutorial Settings")]
    public bool showIntroMessage = true;
    public float waitBeforeNextStep = 0.4f;

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
        foreach (var note in sequence.notes)
        {
            var b = Instantiate(bubblePrefab, bubbleContainer);
            b.GetComponent<NoteBubble>().Setup(note);
        }

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
        // Este tutorial NO usa mecanismos de derrota o estadísticas
        currentIndex = 1;

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
        
        var targetNote = sequence.notes[currentIndex];

        for (int i = 0; i < bubbleContainer.childCount; i++)
        {
            bubbleContainer.GetChild(i)
                .GetComponent<NoteBubble>()
                .Highlight(i == currentIndex);
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
        // Evita tocar si ya está fuera del rango
        if (currentIndex >= sequence.notes.Length)
            return;

        if (pressedNote.noteName != sequence.notes[currentIndex].noteName)
            return;

        StartCoroutine(JumpToNextStep());
    }


    IEnumerator JumpToNextStep()
    {
        PositionFrogAt(currentIndex);
        currentIndex++;

        yield return new WaitForSeconds(waitBeforeNextStep);

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
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialActivityOne : MonoBehaviour
{
    public GameObject bubblePrefab;
    public GameObject frogPrefab;
    public Transform bubbleContainer;
    public PianoKey[] pianoKeys;

    private int currentIndex = 1;

    public LevelSequence sequence;
    private RectTransform frog;

    [Header("Tutorial Settings")]
    public bool showIntroMessage = true;
    public float waitBeforeNextStep = 0.4f;

    void Start()
    {
        GenerateBubbles();
        CreateFrog();
        PositionFrogAt(0);

        SetupTutorialSequence();
        HighlightCurrentBubble();
        LinkPianoKeys();
        TTSManager.Instance.Speak("Ayuda a Rene tocando la tecla correcta en el piano.");
        if (showIntroMessage)
            ShowIntro();
    }

    // ---------------------------------------------------------
    void CreateFrog()
    {
        GameObject frogObj = Instantiate(frogPrefab, bubbleContainer.parent);
        frog = frogObj.GetComponent<RectTransform>();
    }

    void GenerateBubbles()
    {
        foreach (var note in sequence.notes)
        {
            var b = Instantiate(bubblePrefab, bubbleContainer);
            b.GetComponent<NoteBubble>().Setup(note);
        }

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
    void ShowIntro()
    {
        Debug.Log("⭐ Bienvenido al tutorial — Toca la nota correcta para que René salte.");

        // 👈 AUDIO: Instrucción inicial
        TTSManager.Instance.Speak("Ayuda a Rene tocando la tecla correcta en el piano.");
    }

    void SetupTutorialSequence()
    {
        currentIndex = 1;

        foreach (var key in pianoKeys)
            key.SetKeyEnabled(false);
    }

    void LinkPianoKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    // ---------------------------------------------------------------------------
    void HighlightCurrentBubble()
    {
        var targetNote = sequence.notes[currentIndex];

        for (int i = 0; i < bubbleContainer.childCount; i++)
        {
            bubbleContainer.GetChild(i)
                .GetComponent<NoteBubble>()
                .Highlight(i == currentIndex);
        }

        foreach (var key in pianoKeys)
        {
            bool isCorrect = key.noteData.noteName == targetNote.noteName;
            key.SetKeyEnabled(isCorrect);

            if (isCorrect)
                key.ShowHelp();
            else
                key.ResetVisualHelp();
        }

        // 👈 AUDIO: Decir la siguiente nota
        if(currentIndex == 1 ){
            TTSManager.Instance.Speak("Ayuda a Rene tocando la tecla correcta en el piano.");
        }
        TTSManager.Instance.Speak("Toca la nota " + targetNote.noteName);
    }

    // ---------------------------------------------------------------------------
    void OnKeyPressed(NoteData pressedNote)
    {
        if (currentIndex >= sequence.notes.Length)
            return;

        if (pressedNote.noteName != sequence.notes[currentIndex].noteName)
            return;

        StartCoroutine(JumpToNextStep());
    }

    IEnumerator JumpToNextStep()
    {
        PositionFrogAt(currentIndex);
        currentIndex++;

        yield return new WaitForSeconds(waitBeforeNextStep);

        if (currentIndex >= sequence.notes.Length)
        {
            EndTutorial();
            yield break;
        }

        HighlightCurrentBubble();
    }

    // ---------------------------------------------------------------------------
    void EndTutorial()
    {
        Debug.Log("🎉 Tutorial completado, ¡René llegó a su comida!");

        // 👈 AUDIO: Final
        TTSManager.Instance.Speak("Muy bien. Rene llegó a su comida.");
    }
}
