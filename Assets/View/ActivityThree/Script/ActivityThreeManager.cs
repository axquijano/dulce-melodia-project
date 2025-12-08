using UnityEngine;
using System.Collections.Generic;

public class ActivityThreeManager : MonoBehaviour
{
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

    public RectTransform stopPoint;

    public PianoKey[] pianoKeys;

    [Header("Canción (Nivel Fácil)")]
    public List<SongNote> easySongNotes = new List<SongNote>();

    private int currentIndex = 0;
    private FallingNote currentNote;

    void Start()
    {
        LinkPianoKeys();
        SpawnNextNote();
    }

    void LinkPianoKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    // ---------------------------------------------------------------------------
    //  SPAWNEAR UNA NOTA
    // ---------------------------------------------------------------------------
    void SpawnNextNote()
    {
        if (currentIndex >= easySongNotes.Count)
        {
            Debug.Log("Canción terminada!");
            return;
        }

        string noteName = easySongNotes[currentIndex].note.noteName;

        // Obtener el carril correcto
        RectTransform lane = GetLane(noteName);

        // Instanciar la nota
        GameObject noteObj = Instantiate(noteCandyPrefab, lane);
        currentNote = noteObj.GetComponent<FallingNote>();
        string nameStopPoint = "StopPoint_"+noteName;
        currentNote.stopPoint = lane.Find(nameStopPoint).GetComponent<RectTransform>();

        // Posición inicial (arriba del carril)
        RectTransform rect = noteObj.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 300); // ajusta según necesidad

        // Asignar stopPoint
        currentNote.stopPoint = stopPoint; // el Lane debe tener el punto final abajo
    }

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

    // ---------------------------------------------------------------------------
    //     AL TOCAR UNA TECLA
    // ---------------------------------------------------------------------------
    void OnKeyPressed(NoteData pressedNote)
    {
        if (currentNote == null) return;

        if (pressedNote.noteName == easySongNotes[currentIndex].note.noteName)
        {
            // Nota correcta -> destruir y avanzar
            Destroy(currentNote.gameObject);

            currentIndex++;
            SpawnNextNote();
        }
        else
        {
            Debug.Log("Nota incorrecta, intenta de nuevo");
        }
    }
}
