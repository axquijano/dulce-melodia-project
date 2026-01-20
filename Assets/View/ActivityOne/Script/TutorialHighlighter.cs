using UnityEngine;

public class TutorialHighlighter : MonoBehaviour
{
    public Material spotlightMaterial;

    public void Highlight(RectTransform target)
    {
        // Posición en pantalla
        Vector2 screenPos =
            RectTransformUtility.WorldToScreenPoint(null, target.position);

        // Convertimos a UV (0–1)
        Vector2 uv = new Vector2(
            screenPos.x / Screen.width,
            screenPos.y / Screen.height
        );

        spotlightMaterial.SetVector("_Center",
            new Vector4(uv.x, uv.y, 0, 0));

        // Radio basado en tamaño del target
        float radius = Mathf.Max(
            target.rect.width / Screen.width,
            target.rect.height / Screen.height
        ) * 0.6f;

        spotlightMaterial.SetFloat("_Radius", radius);
    }

    public void Clear()
    {
        spotlightMaterial.SetFloat("_Radius", 0f);
    }
}
