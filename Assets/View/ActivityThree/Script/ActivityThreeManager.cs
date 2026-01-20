using UnityEngine;
using System.Collections.Generic;

public class ActivityThreeManager : MonoBehaviour
{
    [Header("Song")]
    public SongData songData;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Prefabs")]
    public GameObject noteCandyPrefab;

    [Header("Lane Targets")]
    public RectTransform laneC;
    public RectTransform laneD;
    public RectTransform laneE;
    public RectTransform laneF;
    public RectTransform laneG;
    public RectTransform laneA;
    public RectTransform laneB;

    [Header("UI Zona de impacto")]
    public RectTransform hitZone;

    [Header("Piano")]
    public PianoKey[] pianoKeys;

    [Header("Notas")]
    public float spawnY = 300f;
    public float noteSpeed = 300f;

    // 🔹 NUEVO (mínimo necesario)
    [Header("Ajuste visual")]
    [Tooltip("Escala el tiempo visual sin cambiar el audio")]
    public float timeScale = 1.5f;

    [Tooltip("Tiempo mínimo entre spawns para evitar notas pegadas")]
    public float minSpawnInterval = 0.6f;

    // ---- Estado interno ----
    private int currentSectionIndex = 0;
    private int nextNoteIndex = 0;
    private float sectionTimer = 0f;
    private float lastSpawnTime = -10f; // 🔹 NUEVO

    private List<FallingNote> activeNotes = new();

    // --------------------------------------------------------
    void Start()
    {
        ActivityConnector.Instance.StartLevel();

        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;

        StartSection(0);
    }

    // --------------------------------------------------------
    void Update()
    {
        if (currentSectionIndex >= songData.sections.Count)
            return;

        sectionTimer += Time.deltaTime;

        HandleSpawning();
        CleanupNotes();
        CheckSectionEnd();
    }

    // --------------------------------------------------------
    void StartSection(int index)
    {
        if (index >= songData.sections.Count)
        {
            Debug.Log("🎉 Canción finalizada");
            return;
        }

        currentSectionIndex = index;
        nextNoteIndex = 0;
        sectionTimer = 0f;
        lastSpawnTime = -10f; // 🔹 reset

        SongSection section = songData.sections[index];

        audioSource.clip = section.referenceAudio;
        audioSource.Play();

        Debug.Log($"▶ Iniciando sección {index}");
    }

    // --------------------------------------------------------
    void HandleSpawning()
    {
        SongSection section = songData.sections[currentSectionIndex];
        TimedNote[] notes = section.row1;

        if (nextNoteIndex >= notes.Length)
            return;

        TimedNote nextNote = notes[nextNoteIndex];

        RectTransform lane = GetLane(nextNote.note.noteName);

        // Caída hasta el centro de la franja
        float hitZoneY = hitZone.anchoredPosition.y;
        float distance = spawnY - hitZoneY;
        float fallTime = distance / noteSpeed;

        // 🔹 TIEMPO VISUAL ESCALADO
        float visualNoteTime = nextNote.time * timeScale;
        float spawnTime = visualNoteTime - fallTime;

        // 🔹 CONTROL DE NOTAS PEGADAS
        if (sectionTimer >= spawnTime &&
            sectionTimer - lastSpawnTime >= minSpawnInterval)
        {
            SpawnNote(nextNote, lane);
            lastSpawnTime = sectionTimer;
            nextNoteIndex++;
        }
    }

    // --------------------------------------------------------
    void SpawnNote(TimedNote timedNote, RectTransform lane)
    {
        GameObject noteObj = Instantiate(noteCandyPrefab, lane);

        FallingNote note = noteObj.GetComponent<FallingNote>();
        RectTransform rect = noteObj.GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(0, spawnY);

        note.rect = rect;
        note.speed = noteSpeed;
        note.hitZone = hitZone;
        note.noteData = timedNote.note;

        activeNotes.Add(note);

#if UNITY_EDITOR
        Debug.Log($"⭐ Spawn {timedNote.note.noteName} | t={timedNote.time:F2}");
#endif
    }

    // --------------------------------------------------------
    void OnKeyPressed(NoteData pressedNote)
    {
        foreach (var note in activeNotes)
        {
            if (note == null) continue;

            if (note.isReadyToHit && note.noteData == pressedNote)
            {
                HandleCorrectHit(note);
                return;
            }
        }

        HandleMiss();
    }

    // --------------------------------------------------------
    void HandleCorrectHit(FallingNote note)
    {
        activeNotes.Remove(note);
        Destroy(note.gameObject);

        Debug.Log("✅ Nota correcta");
    }

    // --------------------------------------------------------
    void HandleMiss()
    {
        Debug.Log("ℹ️ Tecla presionada sin nota activa");
    }

    // --------------------------------------------------------
    void CheckSectionEnd()
    {
        SongSection section = songData.sections[currentSectionIndex];

        if (!audioSource.isPlaying &&
            nextNoteIndex >= section.row1.Length &&
            activeNotes.Count == 0)
        {
            StartSection(currentSectionIndex + 1);
        }
    }

    // --------------------------------------------------------
    void CleanupNotes()
    {
        activeNotes.RemoveAll(n => n == null);
    }

    // --------------------------------------------------------
    RectTransform GetLane(string noteName)
    {
        return noteName switch
        {
            "C" => laneC,
            "D" => laneD,
            "E" => laneE,
            "F" => laneF,
            "G" => laneG,
            "A" => laneA,
            "B" => laneB,
            _ => laneC
        };
    }
}
