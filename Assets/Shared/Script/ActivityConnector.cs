using UnityEngine;
using System;

public class ActivityConnector : MonoBehaviour
{
    public static ActivityConnector Instance;

    private int hits = 0;
    private int mistakes = 0;
    private bool levelWon = false;

    public int Hits => hits;
    public int Mistakes => mistakes;
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

    // ---------------------------
    // CONTROL DEL NIVEL
    // ---------------------------

    public void StartLevel()
    {
        hits = 0;
        mistakes = 0;
        levelWon = false;
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
        levelWon = completed;

        // 🔥 USAMOS EL TIEMPO REAL DEL FEEDBACK MANAGER
        float finalTime = FeedbackManager.Instance.GetTime();

        LevelAttempt attempt = new LevelAttempt
        {
            time = finalTime,
            hits = FeedbackManager.Instance.GetHits(),
            mistakes = FeedbackManager.Instance.GetMistakes(),
            completed = completed,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        LevelData levelData = ProfilesManager.Instance.GetCurrentLevelData();
        levelData.attempts.Add(attempt);

        ProfilesManager.Instance.UpdateCurrentLevelData(levelData);

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
        if (!levelWon)
        {
            SceneLoader.Instance.LoadScene(
                GameFlowManager.Instance.selectedActivity.gameplaySceneName
            );
            return;
        }

        if (!GameFlowManager.Instance.IsLastLevel())
        {
            GameFlowManager.Instance.GoToNextLevel();
            return;
        }

        SceneLoader.Instance.LoadScene("MapEmotions");
    }
}
