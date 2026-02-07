using UnityEngine;
using System;

public class ActivityConnector : MonoBehaviour
{
    public static ActivityConnector Instance;

    private int hits = 0;
    private int mistakes = 0;
    private float timer = 0f;
    private bool playing = false;
    private bool levelWon = false;

    // Lecturas públicas
    public int Hits => hits;
    public int Mistakes => mistakes;
    public float ElapsedTime => timer;
    public bool LevelWon => levelWon;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (playing)
            timer += Time.deltaTime;
    }

    // ---------------------------
    // CONTROL DEL NIVEL
    // ---------------------------

    public void StartLevel()
    {
        hits = 0;
        mistakes = 0;
        timer = 0f;
        levelWon = false;
        playing = true;
    }

    public void RegisterHit()
    {
        hits++;
    }

    public void RegisterMistake()
    {
        mistakes++;
    }

    // ---------------------------
    // RESULTADOS
    // ---------------------------

    public void OnWin()
    {
        FinishLevel(completed: true);
    }

    public void OnLose()
    {
        FinishLevel(completed: false);
    }

    private void FinishLevel(bool completed)
    {
        playing = false;
        levelWon = completed;

        // Crear intento
        LevelAttempt attempt = new LevelAttempt
        {
            time = timer,
            hits = hits,
            mistakes = mistakes,
            completed = completed,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        // Guardar intento
        LevelData levelData = ProfilesManager.Instance.GetCurrentLevelData();
        levelData.attempts.Add(attempt);

        ProfilesManager.Instance.UpdateCurrentLevelData(levelData);

        // Ir a resultados
        SceneLoader.Instance.LoadScene("ActivityResult");
    }

    // ---------------------------
    // NAVEGACIÓN
    // ---------------------------

    public void RetryLevel()
    {
        SceneLoader.Instance.LoadScene(
            GameFlowManager.Instance.selectedActivity.gameplaySceneName
        );
    }



     public void ContinuePlaying()
    {
        // Repetir nivel
        if (!levelWon)
        {
            SceneLoader.Instance.LoadScene(
                GameFlowManager.Instance.selectedActivity.gameplaySceneName
            );
            return;
        }

        // Si gano ir al siguiente nivel
        if (!GameFlowManager.Instance.IsLastLevel())
        {
            GameFlowManager.Instance.GoToNextLevel();
            return;
        }

        // Si es el ultimo nivel de la actividad ir al mapa de emociones
        SceneLoader.Instance.LoadScene("MapEmotions");
    }
}
