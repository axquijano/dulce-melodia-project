using UnityEngine;

public class SpaceRhythmTimeline : MonoBehaviour
{
    public RectTransform notesRoot;

    [Header("Speed")]
    public float pixelsPerSecond = 200f;

    void Update()
    {
        if (HasPendingNote())
            return; // ⛔ la cinta se detiene

        notesRoot.anchoredPosition +=
            Vector2.down * pixelsPerSecond * Time.deltaTime;
    }

    bool HasPendingNote()
    {
        RhythmNoteView[] notes = FindObjectsByType<RhythmNoteView>(FindObjectsSortMode.None);


        foreach (var note in notes)
        {
            if (note.IsPending)
                return true;
        }

        return false;
    }
}
