using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    [TextArea]
    public string message;

    [Tooltip("Flecha u objeto visual a mostrar")]
    public GameObject[] arrowToShow;

    [Tooltip("Tiempo estimado del mensaje (segundos)")]
    public float voiceDuration = 3f;

    [Tooltip("Si es verdadero, el paso espera una acción del niño (ej: tocar una tecla)")]
    public bool waitForAction = false;

    // Método para activar o desactivar todas las flechas
    public void SetArrowsActive(bool isActive)
    {
        if (arrowToShow == null) return;

        foreach (GameObject arrow in arrowToShow)
        {
            if (arrow != null)
                arrow.SetActive(isActive);
        }
    }
}
