using UnityEngine;

public class ActivityConnector : MonoBehaviour
{
    public static ActivityConnector Instance;

    [HideInInspector] public bool levelWon;

    private ActivityDefinition activity;
    private int level;
    public int hits;
    public int mistakes;
    public float time;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);   // 🔥 IMPORTANTE 🔥
    }
    
    void Start()
    {
        activity = GameFlowManager.Instance.selectedActivity;
        level = GameFlowManager.Instance.selectedLevel;
    }

   public void OnWin()
    {
        levelWon = true;
        StorePerformance();
        SceneLoader.Instance.LoadScene("ActivityResult");
    }

    public void OnLose()
    {
        levelWon = false;
        StorePerformance();
        SceneLoader.Instance.LoadScene("ActivityResult");
    }

    void StorePerformance()
    {
        hits = FeedbackManager.Instance.GetHits();
        mistakes = FeedbackManager.Instance.getMistakes();
        time = FeedbackManager.Instance.GetTime();
    }


    void SavePerformance(bool win)
    {
        float time = FeedbackManager.Instance.GetTime();
        int hits = FeedbackManager.Instance.GetHits();
        int mistakes = FeedbackManager.Instance.getMistakes();

        ProfilesManager.Instance.UpdateLevelData(
            level,
            time,
            hits,
            mistakes,
            win
        );
    }

    public void BackToMap()
    {
        SceneLoader.Instance.LoadScene("MapActivity");
    }
}
