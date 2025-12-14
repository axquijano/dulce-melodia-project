using UnityEngine;
using TMPro;

public class ResultStatsUI : MonoBehaviour
{
    [Header("UI References Won")]
    public TMP_Text hitsText;
    public TMP_Text timeText;
    public TMP_Text mistakesText;


    [Header("UI References Lose")]
    public TMP_Text hitsTextLose;
    public TMP_Text timeTextLose;
    public TMP_Text mistakesTextLose;


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
        int mistakes = ActivityConnector.Instance.Mistakes;
        float time = ActivityConnector.Instance.ElapsedTime;

        // Mostramos en UI
        hitsText.text = hits.ToString();
        mistakesText.text = mistakes.ToString();
        timeText.text = FormatTime(time);

        hitsTextLose.text = hits.ToString();
        mistakesTextLose.text = mistakes.ToString();
        timeTextLose.text = FormatTime(time);
    }

    string FormatTime(float t)
    {
        int min = Mathf.FloorToInt(t / 60f);
        int sec = Mathf.FloorToInt(t % 60f);
        return $"{min:00}:{sec:00}";
    }
}
