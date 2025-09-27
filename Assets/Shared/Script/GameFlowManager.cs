using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    public ActivityDefinition selectedActivity;
    public int selectedLevel;
     public ActivitiesDatabase database;

    private void Awake() => Instance = this;

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
}
