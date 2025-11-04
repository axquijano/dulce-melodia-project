using UnityEngine;

public class SpaceRhythmBuilder : MonoBehaviour
{
    public SongData songData;

    [Header("Lanes")]
    public RectTransform laneC;
    public RectTransform laneD;
    public RectTransform laneE;
    public RectTransform laneF;
    public RectTransform laneG;
    public RectTransform laneA;
    public RectTransform laneB;

    public GameObject starNotePrefab;

    [Header("Spacing")]
    public float pixelsPerSecond = 200f;
    public float startOffset = 800f; // qué tan arriba inicia la canción

    void Start()
    {
        BuildSection(0);
    }

    void BuildSection(int sectionIndex)
    {
        SongSection section = songData.sections[sectionIndex];

        BuildRow(section.row1);
        BuildRow(section.row2);
    }

    void BuildRow(TimedNote[] notes)
    {
        foreach (var note in notes)
        {
            RectTransform lane = GetLane(note.note.noteName);
            CreateNote(note, lane);
        }
    }

    void CreateNote(TimedNote timedNote, RectTransform lane)
    {
        GameObject obj = Instantiate(starNotePrefab, lane);
        RectTransform rect = obj.GetComponent<RectTransform>();

        // ⬆️ lo primero empieza más arriba
        float y = startOffset + (timedNote.time * pixelsPerSecond);
        rect.anchoredPosition = new Vector2(0, y);

        StarNoteUI ui = obj.GetComponent<StarNoteUI>();
        ui.noteData = timedNote.note;
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
}
