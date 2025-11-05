using UnityEngine;

public class SpaceRhythmManager : MonoBehaviour
{
    public static SpaceRhythmManager Instance;

    [Header("Piano")]
    public PianoKey[] pianoKeys;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LinkPianoKeys();
        DisableInternalKeySounds();
    }

    void DisableInternalKeySounds()
    {
        foreach (var key in pianoKeys)
        {
            key.allowInternalSound = false;
        }
    }


    void LinkPianoKeys()
    {
        foreach (var key in pianoKeys)
        {
            key.onKeyPressed += OnKeyPressed;
        }
    }

    void OnKeyPressed(NoteData pressedNote)
    {
        NoteStar pending = GetPendingNote();

        // 🎹 tecla libre (no hay nota esperando)
        if (pending == null)
        {
            // el sonido ya lo maneja PianoKey
            return;
        }

        // ✅ nota correcta
        if (pending.noteData == pressedNote)
        {
            pending.Consume(); // libera la cinta
        }
        else
        {
            // ❌ incorrecta → no pasa nada
            // la cinta sigue detenida
        }
    }

    NoteStar GetPendingNote()
    {
        NoteStar[] notes = FindObjectsOfType<NoteStar>();

        foreach (var note in notes)
        {
            if (note.IsPending)
                return note;
        }

        return null;
    }
}
