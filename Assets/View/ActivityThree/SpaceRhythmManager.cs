using UnityEngine;

public class SpaceRhythmManager : MonoBehaviour
{
    public static SpaceRhythmManager Instance;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Piano")]
    public PianoKey[] pianoKeys;

    private int totalNotes;
    private int consumedNotes;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FeedbackManager.Instance.ResetStats();
        FeedbackManager.Instance.SetMaxMistakes(12); // 🔵 barra máxima
        ActivityConnector.Instance.StartLevel();

        LinkPianoKeys();
        DisableInternalKeySounds();

        totalNotes = FindObjectsByType<RhythmNoteView>(
            FindObjectsSortMode.None
        ).Length;

        consumedNotes = 0;
    }

    void DisableInternalKeySounds()
    {
        foreach (var key in pianoKeys)
            key.allowInternalSound = false;
    }

    void LinkPianoKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    void OnKeyPressed(NoteData pressedNote)
    {
        RhythmNoteView pending = GetPendingNote();

        // ❌ Si no hay nota en zona → error
        if (pending == null)
        {
            RegisterMistake();
            return;
        }

        // ✅ Nota correcta
        if (pending.timedNote.note == pressedNote)
        {
            RegisterHit();

            PlayTimedNoteSound(pending.timedNote);
            pending.Consume();

            consumedNotes++;

            CheckGameEnd();
        }
        else
        {
            RegisterMistake();
        }
    }

    RhythmNoteView GetPendingNote()
    {
        foreach (var note in FindObjectsByType<RhythmNoteView>(
            FindObjectsSortMode.None))
        {
            if (note.IsPending)
                return note;
        }

        return null;
    }

    void RegisterHit()
    {
        FeedbackManager.Instance.RegisterHit();
        ActivityConnector.Instance.RegisterHit();
    }

    void RegisterMistake()
    {
        FeedbackManager.Instance.RegisterMistake();
        ActivityConnector.Instance.RegisterMistake();

        // 🔴 Si se acabó la barra → pierde
        if (ActivityConnector.Instance.Mistakes >= 12)
        {
            Debug.Log("❌ Nivel perdido");
            ActivityConnector.Instance.OnLose();
        }
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

    void CheckGameEnd()
    {
        if (consumedNotes >= totalNotes)
        {
            Debug.Log("🎉 Nivel completado");

            Debug.Log("Aciertos: " + FeedbackManager.Instance.GetHits());
            Debug.Log("Errores: " + FeedbackManager.Instance.GetMistakes());

            ActivityConnector.Instance.OnWin();
        }
    }
}
