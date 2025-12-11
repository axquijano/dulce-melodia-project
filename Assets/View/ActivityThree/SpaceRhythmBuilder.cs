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
    public float startOffset = 800f;

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
        foreach (var timedNote in notes)
        {
            RectTransform lane = GetLane(timedNote.note.noteName);
            CreateNote(timedNote, lane);
        }
    }

    void CreateNote(TimedNote timedNote, RectTransform lane)
    {
        GameObject obj = Instantiate(starNotePrefab, lane);
        RectTransform rect = obj.GetComponent<RectTransform>();

        // ⬆️ Las primeras notas quedan más arriba
        float y = startOffset + (timedNote.time * pixelsPerSecond);
        rect.anchoredPosition = new Vector2(0, y);

        // ⭐ Inicializamos NoteStar
        RhythmNoteView noteStar = obj.GetComponent<RhythmNoteView>();
        noteStar.Setup(timedNote);
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
            _   => laneC
        };
    }
}
