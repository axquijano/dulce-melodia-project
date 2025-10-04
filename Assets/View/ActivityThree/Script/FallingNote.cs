using UnityEngine;
public class FallingNote : MonoBehaviour
{
    public float speed = 400f;
    public bool isStopped = false;
    public RectTransform rect;
    public RectTransform stopPoint;

    void Update()
    {
        if (isStopped) return;

        rect.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);

        if (rect.anchoredPosition.y <= stopPoint.anchoredPosition.y)
        {
            Debug.Log("Nota llegó al punto de parada");
            isStopped = true; // Se detiene cuando llega al target
        }
    }
}

