using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelRowUI : MonoBehaviour
{
    public TMP_Text levelNumber;
    public TMP_Text status;
    public TMP_Text best;
    public TMP_Text worst;
    public TMP_Text speed;
    public TMP_Text retries;

    public Image statusIcon;

    public void Setup(
        LevelData level,
        bool activityUnlocked,
        int levelIndex,
        int currentLevelIndex,
        StatusIconDatabase statusIconDatabase
    )
    {
        levelNumber.text = (levelIndex + 1).ToString();

        string statusText;

        if (!activityUnlocked)
        {
            statusText = "Bloqueado";
        }
        else if (currentLevelIndex == -1)
        {
            statusText = "Superado";
        }
        else if (levelIndex == currentLevelIndex)
        {
            statusText = "En curso";
        }
        else if (level.CompletedAtLeastOnce())
        {
            statusText = "Superado";
        }
        else
        {
            statusText = "Bloqueado";
        }

        status.text = statusText;
        statusIcon.sprite = statusIconDatabase.GetIcon(statusText);
        statusIcon.enabled = statusIcon.sprite != null;

        if (level.attempts.Count > 0)
        {
            best.text = $"{StatsCalculator.GetBestAttempt(level):0}%";
            worst.text = $"{StatsCalculator.GetWorstAttempt(level):0}%";
            speed.text = StatsCalculator.GetSpeedImprovement(level);
            retries.text = level.attempts.Count.ToString();
        }
        else
        {
            best.text = "-";
            worst.text = "-";
            speed.text = "-";
            retries.text = "0";
        }
    }
}
