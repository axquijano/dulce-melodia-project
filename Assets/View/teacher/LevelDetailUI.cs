using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelDetailUI : MonoBehaviour
{
    public TMP_Text nameActivity;
    public Image iconActivity;

    public ActivitiesDatabase activitiesDatabase;
    public StatusIconDatabase statusIconDatabase;

    public LevelRowUI levelOne;
    public LevelRowUI levelTwo;
    public LevelRowUI levelThree;

    public void Setup(ActivityEntry activity)
    {
        // 🔹 Definición de la actividad
        ActivityDefinition definition = activitiesDatabase.GetById(activity.key);

        if (definition != null)
        {
            nameActivity.text = definition.activityName;
            iconActivity.sprite = definition.icon;
            iconActivity.enabled = true;
        }
        else
        {
            nameActivity.text = activity.key;
            iconActivity.enabled = false;
        }

        // 🔹 Nivel actual (solo uno en curso)
        int currentLevelIndex = activity.value.GetCurrentLevelIndex();

        levelOne.Setup(activity.value.levels[0], activity.unlocked, 0, currentLevelIndex, statusIconDatabase);
        levelTwo.Setup(activity.value.levels[1], activity.unlocked, 1, currentLevelIndex, statusIconDatabase);
        levelThree.Setup(activity.value.levels[2], activity.unlocked, 2, currentLevelIndex, statusIconDatabase);
    }
}
