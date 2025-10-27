using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivityRowUI : MonoBehaviour
{
    [Header("Activity")]
    public Image activityIcon;
    public TMP_Text activityName;

    [Header("Status")]
    public Image statusIcon;
    public TMP_Text statusText;

    [Header("Stats")]
    public TMP_Text domainText;
    public TMP_Text timeText;

    [Header("Emotion")]
    public Image emotionIcon;
    public TMP_Text emotionText;

    [Header("Databases")]
    public ActivitiesDatabase activitiesDatabase;
    public StatusIconDatabase statusIconDatabase;
    public EmotionIconDatabase emotionIconDatabase;

    public void Setup(ActivityEntry activity, string status)
    {
        ActivityDefinition def = activitiesDatabase.GetById(activity.key);

        if (def == null)
        {
            Debug.LogWarning($"No ActivityDefinition for {activity.key}");
            return;
        }

        activityName.text = def.activityName;
        activityIcon.sprite = def.icon;

        statusText.text = status;
        statusIcon.sprite = statusIconDatabase.GetIcon(status);
        statusIcon.enabled = statusIcon.sprite != null;

        domainText.text = $"{StatsCalculator.GetGlobalDomain(activity):0}%";
        timeText.text = StatsCalculator.FormatTime(
            StatsCalculator.GetTotalTime(activity)
        );

        if (string.IsNullOrEmpty(activity.lastSelectedEmotion))
        {
            emotionText.text = "--";
            emotionIcon.enabled = false;
        }
        else
        {
            emotionText.text = activity.lastSelectedEmotion;
            emotionIcon.sprite =
                emotionIconDatabase.GetIcon(activity.lastSelectedEmotion);
            emotionIcon.enabled = true;
        }
    }
}
