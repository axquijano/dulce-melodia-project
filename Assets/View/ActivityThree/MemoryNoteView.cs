using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MemoryNoteView : NoteView
{
    bool isGhostType;      // Esta nota puede ser ghost
    bool ghostActive;      // Actualmente está oculta
    Coroutine pulseRoutine;

    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // --------------------------------------------------
    // SETUP
    // --------------------------------------------------
    public void Setup(NoteData data, bool ghost)
    {
        base.Init(data);
        isGhostType = ghost;
        ghostActive = false;
        ResetVisual();
    }

    // --------------------------------------------------
    // ESCUCHA (autoplay)
    // --------------------------------------------------
    public void ShowListening()
    {
        StopPulse();
        SetSprite(noteData.imagenStar);
        SetLabelColor(noteData.color);
    }

    // --------------------------------------------------
    // TURNO DEL NIÑO
    // --------------------------------------------------
    public void ShowTurn()
    {
        StopPulse();
        pulseRoutine = StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        while (true)
        {
            transform.localScale = Vector3.one * 1.1f;
            yield return new WaitForSeconds(0.4f);
            transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.4f);
        }
    }

    void StopPulse()
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }

        transform.localScale = Vector3.one;
    }

    // --------------------------------------------------
    // CORRECTO
    // --------------------------------------------------
    public void ShowCorrect()
    {
        StopPulse();
        ghostActive = false;

        SetSprite(noteData.imagenStar);
        SetLabelColor(Color.white);

        StartCoroutine(PopEffect());
    }

    IEnumerator PopEffect()
    {
        transform.localScale = Vector3.one * 1.2f;
        yield return new WaitForSeconds(0.15f);
        transform.localScale = Vector3.one;
    }

    // --------------------------------------------------
    // ERROR
    // --------------------------------------------------
    public void ShowError()
    {
        StopPulse();
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 original = transform.localPosition;

        for (int i = 0; i < 6; i++)
        {
            transform.localPosition = original + Vector3.right * 5f;
            yield return new WaitForSeconds(0.05f);
            transform.localPosition = original - Vector3.right * 5f;
            yield return new WaitForSeconds(0.05f);
        }

        transform.localPosition = original;
    }

    // --------------------------------------------------
    // RESET
    // --------------------------------------------------
    public void ResetVisual()
    {
        StopPulse();

        if (ghostActive)
        {
            label.text = "?";
            SetLabelColor(Color.black);
        }
        else
        {
            label.text = noteData.noteName;
            SetLabelColor(noteData.color);
        }

        SetSprite(defaultSprite);
        transform.localScale = Vector3.one;
    }

    // --------------------------------------------------
    // ACTIVAR GHOST (CON ANIMACIÓN)
    // --------------------------------------------------
    public void ActivateGhost()
    {
        if (!isGhostType) return;

        ghostActive = true;
        StartCoroutine(GhostAnimation());
    }

    IEnumerator GhostAnimation()
    {
        // Fade out suave
        float t = 0;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / 0.3f);
            yield return null;
        }

        label.text = "?";
        SetLabelColor(Color.black);
        SetSprite(defaultSprite);

        // Fade in suave
        t = 0;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / 0.3f;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }
}
