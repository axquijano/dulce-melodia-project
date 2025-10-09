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

    public PianoKey[] pianoKeys;

    [Header("Canción (Nivel Fácil)")]
    public List<SongNote> easySongNotes = new List<SongNote>();

  
    [Header("Notas")]
    public float spawnY = 300f;
    public float noteSpeed = 300f;

    private float songTimer = 0f;
    private int nextSpawnIndex = 0;

    void Start()
    {
        ActivityConnector.Instance.StartLevel();
        // Vincular piano
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;

        Debug.Log("---- INICIO DE SIMULACIÓN ----");
    }

    void Update()
    {
        songTimer += Time.deltaTime;

        if (nextSpawnIndex < easySongNotes.Count)
        {
            SongNote nextNote = easySongNotes[nextSpawnIndex];

            RectTransform lane = GetLane(nextNote.note.noteName);
            RectTransform stop = lane.Find("StopPoint_" + nextNote.note.noteName).GetComponent<RectTransform>();

            float distance = spawnY - stop.anchoredPosition.y;
            float fallTime = distance / noteSpeed;

            float spawnTime = nextNote.time - fallTime;

            // Cuando llega el tiempo exacto de spawn
            if (songTimer >= spawnTime)
            {
                SpawnNote(nextNote, lane, stop, fallTime);
                nextSpawnIndex++;
            }
        }
    }

    // --------------------------------------------------------
    void SpawnNote(SongNote songNote, RectTransform lane, RectTransform stopPoint, float fallTime)
    {
        // Instanciar nota
        GameObject noteObj = Instantiate(noteCandyPrefab, lane);
        FallingNote note = noteObj.GetComponent<FallingNote>();

        RectTransform rect = noteObj.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, spawnY);

        note.rect = rect;
        note.stopPoint = stopPoint;
        note.speed = noteSpeed;

        // ---- LOG DE VERIFICACIÓN ----
        float expectedArrival = songNote.time;
        float realArrival = songTimer + fallTime;

        Debug.Log(
            $"<color=yellow>♫ Spawn de nota {songNote.note.noteName}</color>\n" +
            $"→ Tiempo actual: {songTimer:F2}s\n" +
            $"→ Debe llegar en: {expectedArrival:F2}s\n" +
            $"→ Llegará en: {realArrival:F2}s\n" +
            $"→ Diferencia: {(realArrival - expectedArrival):F3}s\n" +
            $"→ Distancia: {spawnY - stopPoint.anchoredPosition.y:F2}px\n" +
            $"→ Tiempo de caída real: {fallTime:F3}s"
        );
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

    // --------------------------------------------------------
    void OnKeyPressed(NoteData pressedNote)
    {
        Debug.Log("Input deshabilitado en esta versión de debug.");
    }
}