using UnityEngine;
using TMPro;

public class ResultStatsUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text hitsText;
    public TMP_Text timeText;

    void Start()
    {
        // Validamos conexión
        if (ActivityConnector.Instance == null)
        {
            Debug.LogError("ActivityConnector no existe en esta escena.");
            return;
        }

        // Obtenemos los datos guardados del juego
        int hits = ActivityConnector.Instance.Hits;
        float time = ActivityConnector.Instance.ElapsedTime;

        // Mostramos en UI
        hitsText.text = hits.ToString();
        timeText.text = FormatTime(time);
    }

    string FormatTime(float t)
    {
        int min = Mathf.FloorToInt(t / 60f);
        int sec = Mathf.FloorToInt(t % 60f);
        return $"{min:00}:{sec:00}";
    }
}
