using UnityEngine;

public class FallingNote : MonoBehaviour
{
    public float speed;
    public RectTransform rect;
    public RectTransform hitZone;     // franja amarilla
    public NoteData noteData;

    public bool isReadyToHit = false;
    public bool hasPassed = false;

    float destroyY = -600f;

    void Update()
    {
        // Movimiento
        rect.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);

        CheckHitZone();

        // Destruir fuera de pantalla
        if (rect.anchoredPosition.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }

    void CheckHitZone()
    {
        float noteY = rect.anchoredPosition.y;

        float zoneTop = hitZone.anchoredPosition.y + hitZone.rect.height / 2f;
        float zoneBottom = hitZone.anchoredPosition.y - hitZone.rect.height / 2f;

        if (noteY <= zoneTop && noteY >= zoneBottom)
        {
            isReadyToHit = true;
        }
        else
        {
            // Si ya pasó la zona
            if (noteY < zoneBottom)
                hasPassed = true;

            isReadyToHit = false;
        }
    }
}
