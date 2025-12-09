using UnityEngine;

public class FallingNote : MonoBehaviour
{
    public float speed = 0f;            // px/s calculado según tiempo
    public bool isStopped = false;
    public bool isReadyToHit = false;   // solo true cuando llega al StopPoint
    public RectTransform rect;
    public RectTransform stopPoint;
    public NoteData noteData;

    void Update()
    {
        if (isStopped) return;

        rect.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);

        // Cuando llega al StopPoint se detiene
        if (rect.anchoredPosition.y <= stopPoint.anchoredPosition.y)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, stopPoint.anchoredPosition.y);
            isStopped = true;
            isReadyToHit = true;
        }
    }
}


