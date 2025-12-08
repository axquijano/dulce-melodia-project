using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Win UI")]
    public TMP_Text hitsTextWin;
    public TMP_Text timeTextWin;
    public TMP_Text mistakesTextWin;

    [Header("Lose UI")]
    public TMP_Text hitsTextLose;
    public TMP_Text timeTextLose;
    public TMP_Text mistakesTextLose;

    [Header("Avatar Feedback")]
    public Image avatarImage;

    [Header("Avatar Data")]
    public StudentAvatarDatabase avatarDatabase;

    [Header("Medal")]
    public Image medalImage;
    public ActivityMedalDatabase medalDatabase;

    private StudentAvatarData currentAvatar;
    private string childName;

    void Start()
    {
        var connector = ActivityConnector.Instance;
        if (connector == null)
        {
            Debug.LogError("ActivityConnector no existe.");
            return;
        }

        // Perfil actual
        var profile = ProfilesManager.Instance.currentProfile;
        childName = profile.childName;
        currentAvatar = avatarDatabase.GetById(profile.avatarId);

        bool won = connector.LevelWon;

        winPanel.SetActive(won);
        losePanel.SetActive(!won);

        // 🔥 AHORA TODO SALE DEL FEEDBACK MANAGER
        int hits = FeedbackManager.Instance.GetHits();
        int mistakes = FeedbackManager.Instance.GetMistakes();
        float time = FeedbackManager.Instance.GetTime();

        if (won)
        {
            hitsTextWin.text = hits.ToString();
            mistakesTextWin.text = mistakes.ToString();
            timeTextWin.text = FormatTime(time);

            avatarImage.sprite = currentAvatar.celebrationSprite;

            string text = $"¡Excelente {childName}! Lo hiciste muy bien. ¡Vamos por el siguiente nivel!";
            TTSManager.Instance.Speak(text);
        }
        else
        {
            hitsTextLose.text = hits.ToString();
            mistakesTextLose.text = mistakes.ToString();
            timeTextLose.text = FormatTime(time);

            avatarImage.sprite = currentAvatar.sadSprite;

            string text = $"{childName}, lo intentaste. ¡Vamos a seguir practicando!";
            TTSManager.Instance.Speak(text);
        }

        ShowActivityMedal();
    }

    // --------------------------------------------------
    // MEDAL LOGIC
    // --------------------------------------------------

    void ShowActivityMedal()
    {
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");
        var activity = ProfilesManager.Instance.currentProfile.activities[activityIndex];

        var medalData = medalDatabase.GetByActivityKey(activity.key);

        if (medalData == null)
        {
            medalImage.gameObject.SetActive(false);
            return;
        }

        int completedLevels = GetCompletedLevelsInActivity(activity);

        if (completedLevels == 0)
        {
            medalImage.gameObject.SetActive(false);
        }
        else if (completedLevels == 1)
        {
            medalImage.sprite = medalData.medal_1_3;
            medalImage.gameObject.SetActive(true);
        }
        else if (completedLevels == 2)
        {
            medalImage.sprite = medalData.medal_2_3;
            medalImage.gameObject.SetActive(true);
        }
        else
        {
            medalImage.sprite = medalData.medal_3_3;
            medalImage.gameObject.SetActive(true);
        }
    }

    int GetCompletedLevelsInActivity(ActivityEntry activity)
    {
        int completed = 0;

        foreach (var level in activity.value.levels)
        {
            if (level.CompletedAtLeastOnce())
                completed++;
        }

        return completed;
    }

    // --------------------------------------------------
    // UTILS
    // --------------------------------------------------

    string FormatTime(float t)
    {
        int min = Mathf.FloorToInt(t / 60f);
        int sec = Mathf.FloorToInt(t % 60f);
        return $"{min:00}:{sec:00}";
    }

    public void ContinuePlaying()
    {
        ActivityConnector.Instance.ContinuePlaying();
    }
}
