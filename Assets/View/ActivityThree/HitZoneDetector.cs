using UnityEngine;
using System.Collections.Generic;

public class HitZoneDetector : MonoBehaviour
{
    public RectTransform hitZoneRect;

    private List<StarNoteUI> activeNotes = new();

    void Update()
    {
        UpdateNotes();
    }

    void UpdateNotes()
    {
        StarNoteUI[] notes = FindObjectsOfType<StarNoteUI>();

        foreach (var note in notes)
        {
            bool inside = IsInsideHitZone(note.Rect);

            note.SetActive(inside);
        }
    }

    bool IsInsideHitZone(RectTransform noteRect)
    {
        Vector3[] hitCorners = new Vector3[4];
        Vector3[] noteCorners = new Vector3[4];

        hitZoneRect.GetWorldCorners(hitCorners);
        noteRect.GetWorldCorners(noteCorners);

        float hitTop = hitCorners[1].y;
        float hitBottom = hitCorners[0].y;

        float noteY = noteCorners[0].y;

        return noteY <= hitTop && noteY >= hitBottom;
    }
}
