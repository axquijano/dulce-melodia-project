using UnityEngine;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    [Header("UI")]
    public ProgressBar progressBar;
    public TMP_Text timeText;

    private int hits;
    private int mistakes;
    private float maxMistakes;

    // ---------------------------
    // TIMER
    // ---------------------------
    private float playTimer = 0f;

    private bool manualTimerMode = false;   // 🔹 Nuevo
    private bool timerRunning = true;       // 🔹 Por defecto corre (modo automático)

    public float GetTime() => playTimer;
    public int GetHits() => hits;
    public int GetMistakes() => mistakes;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Update()
    {
        // 🔹 Si está en modo manual, solo corre cuando se active
        if (manualTimerMode)
        {
            if (!timerRunning) return;
        }

        playTimer += Time.deltaTime;

        UpdateTimeUI();
    }

    void UpdateTimeUI()
    {
        if (timeText == null) return;

        int minutes = Mathf.FloorToInt(playTimer / 60f);
        int seconds = Mathf.FloorToInt(playTimer % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
        Debug.Log($"Tiempo real: {minutes:00}:{seconds:00}");
    }

    // =========================================================
    // TIMER CONTROL (NUEVO SISTEMA)
    // =========================================================

    /// <summary>
    /// Activa el modo manual (el tiempo deja de correr automáticamente)
    /// </summary>
    public void EnableManualTimerMode()
    {
        manualTimerMode = true;
        timerRunning = false;
    }

    /// <summary>
    /// Vuelve al modo automático (compatibilidad con juegos anteriores)
    /// </summary>
    public void EnableAutomaticTimerMode()
    {
        manualTimerMode = false;
        timerRunning = true;
    }

    public void StartTimer()
    {
        if (manualTimerMode)
            timerRunning = true;
    }

    public void StopTimer()
    {
        if (manualTimerMode)
            timerRunning = false;
    }

    public void ResetTimer()
    {
        playTimer = 0f;
        UpdateTimeUI();
    }

    // =========================================================
    // PROGRESO
    // =========================================================

    public void SetMaxMistakes(float value)
    {
        maxMistakes = value;

        if (progressBar != null)
            progressBar.SetMaxMistakes(value);
    }

    public void RegisterHit()
    {
        hits++;
        UpdateUI();
    }

    public void RegisterMistake()
    {
        mistakes++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (progressBar != null)
            progressBar.UpdateProgressBar(mistakes);
    }

    public void ResetStats()
    {
        hits = 0;
        mistakes = 0;
        playTimer = 0f;
        timerRunning = !manualTimerMode;
        UpdateTimeUI();
    }
}
