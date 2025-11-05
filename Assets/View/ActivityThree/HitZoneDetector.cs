using UnityEngine;

public class HitZoneDetector : MonoBehaviour
{
    public RectTransform hitZoneRect;

    void Update()
    {
        UpdateNotes();
    }

    void UpdateNotes()
    {
        NoteStar[] notes = FindObjectsOfType<NoteStar>();

        foreach (var note in notes)
        {
            bool inside = IsInsideHitZone(note.GetComponent<RectTransform>());

            if (inside && !note.IsPending)
                note.EnterHitZone();
            else if (!inside && note.IsPending)
                note.ExitHitZone();
        }
    }


    bool IsInsideHitZone(RectTransform noteRect)
    {
        Vector3[] hitCorners = new Vector3[4];
        hitZoneRect.GetWorldCorners(hitCorners);

        float hitTop = hitCorners[1].y;
        float hitBottom = hitCorners[0].y;

        // se valida que el centro de la nota esté dentro del rango vertical del hit zone
        Vector3 noteCenter = noteRect.TransformPoint(noteRect.rect.center);
        float noteCenterY = noteCenter.y;

        return noteCenterY <= hitTop && noteCenterY >= hitBottom;
    }

}
