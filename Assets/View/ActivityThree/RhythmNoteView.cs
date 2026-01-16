using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmNoteView : NoteView
{
    public bool IsPending { get; private set; }
    public TimedNote timedNote;

    // ================================
    // Inicialización
    // ================================
    public void Setup(TimedNote timed)
    {
        timedNote = timed;
        base.Init(timed.note);

        // ⭐ Comienza con estrella visible
        SetSprite(noteData.imagenStar);

        // 🎨 Comienza con su color real
        SetLabelColor(noteData.color);

        IsPending = false; // 🔥 NO comienza pendiente
    }

    // ================================
    // Cuando entra al Hit Zone
    // ================================
    public void EnterHitZone()
    {
        IsPending = true;

        // Se ilumina
        SetLabelColor(Color.white);
    }

    // ================================
    // Cuando sale del Hit Zone
    // ================================
    public void ExitHitZone()
    {
        IsPending = false;

        // Vuelve a su color original
        SetLabelColor(noteData.color);
    }

    // ================================
    // Cuando el jugador acierta
    // ================================
    public void Consume()
    {
        IsPending = false;
        Destroy(gameObject);
    }
}
