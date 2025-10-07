using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    public ActivityDefinition selectedActivity;
    public int selectedLevel;
    public ActivitiesDatabase database;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Reconstruir estado con PlayerPrefs si ya existía
        if (PlayerPrefs.HasKey("CurrentActivity"))
        {
            int a = PlayerPrefs.GetInt("CurrentActivity");
            int l = PlayerPrefs.GetInt("CurrentLevel");

            selectedActivity = database.activities[a];
            selectedLevel = l;
        }
    }


    public void SelectActivity(int activityIndex)
    {
        selectedActivity = database.activities[activityIndex];
        SceneLoader.Instance.LoadScene("MapLevel");
    }

    public void SelectLevel(int levelIndex)
    {
        selectedLevel = levelIndex;
        SceneLoader.Instance.LoadScene(selectedActivity.gameplaySceneName);
    }

    public LevelSequence GetCurrentLevelSequence()
    {
        return selectedActivity.levels[selectedLevel];
    }

    public LevelSettings GetCurrentLevelSettings()
    {
        return selectedActivity.levelSettings[selectedLevel];
    }
}
