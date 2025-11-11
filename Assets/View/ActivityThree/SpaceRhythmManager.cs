using UnityEngine;

public class SpaceRhythmManager : MonoBehaviour
{
    public static SpaceRhythmManager Instance;

    [Header("Audio")]
    public AudioSource audioSource; 

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

        if (pending == null)
            return;

        if (pending.noteData == pressedNote)
        {
            PlayTimedNoteSound(pending.timedNote);
            pending.Consume(); // 🔓 libera la cinta
        }
    }

    NoteStar GetPendingNote()
    {
        foreach (var note in FindObjectsOfType<NoteStar>())
        {
            if (note.IsPending)
                return note;
        }
        return null;
    }

    void PlayTimedNoteSound(TimedNote timedNote)
    {
        AudioClip clip =
            timedNote.overrideSound != null
            ? timedNote.overrideSound
            : timedNote.note.sound;

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}