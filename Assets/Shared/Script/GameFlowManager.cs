using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    public ActivityDefinition selectedActivity;
    public int selectedLevel;
    public ActivitiesDatabase database;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SelectActivity(int activityIndex)
    {
        selectedActivity = database.activities[activityIndex];
        PlayerPrefs.SetInt("CurrentActivity", activityIndex);

        ActivityEntry activity =
            ProfilesManager.Instance.currentProfile.activities[activityIndex];

        // Tutorial
        if (!activity.value.tutorialSeen)
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
            PlayerPrefs.Save();
            SceneLoader.Instance.LoadScene(selectedActivity.tutorialSceneName);
            return;
        }

        // Nivel correcto
        selectedLevel = GetCurrentLevelIndex(activity);
        PlayerPrefs.SetInt("CurrentLevel", selectedLevel);
        PlayerPrefs.Save();

        LoadGameplayScene();
    }

    public int GetCurrentLevelIndex(ActivityEntry activity)
    {
        for (int i = 0; i < activity.value.levels.Count; i++)
        {
            if (!activity.value.levels[i].CompletedAtLeastOnce())
                return i;
        }
        return 0;
    }

    public LevelSequence GetCurrentLevelSequence()
    {
        return selectedActivity.levels[selectedLevel];
    }

    public bool IsLastLevel()
    {
        return selectedLevel >= selectedActivity.levels.Count - 1;
    }

    public void GoToNextLevel()
    {
        selectedLevel++;

        PlayerPrefs.SetInt("CurrentLevel", selectedLevel);
        PlayerPrefs.Save();

        LoadGameplayScene();
    }
    public void LoadGameplayScene()
    {
        // Caso especial: tercer nivel con escena distinta
        if (!string.IsNullOrEmpty(selectedActivity.thirdLevelSceneName)
            && selectedLevel == 2)
        {
            SceneLoader.Instance.LoadScene(selectedActivity.thirdLevelSceneName);
            return;
        }

        // Caso normal
        SceneLoader.Instance.LoadScene(selectedActivity.gameplaySceneName);
    }

}
