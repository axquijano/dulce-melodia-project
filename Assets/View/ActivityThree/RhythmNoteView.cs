using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmNoteView : NoteView
{
    public bool IsPending { get; private set; }
    public TimedNote timedNote;

    bool isActive = false;

    public void Setup(TimedNote timed)
    {
        timedNote = timed;
        base.Init(timed.note);
        SetInactive();
    }

    public void SetActive()
    {
        isActive = true;
        IsPending = true;
        SetSprite(noteData.imagenStar);
        SetLabelColor(Color.white);
    }

    public void SetInactive()
    {
        isActive = false;
        IsPending = false;
        SetSprite(defaultSprite);
        SetLabelColor(noteData.color);
    }

    public void EnterHitZone()
    {
        SetActive();
    }

    public void ExitHitZone()
    {
        SetInactive();
    }

    public void Consume()
    {
        IsPending = false;
        Destroy(gameObject);
    }
}
