using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    [Header("Continue Button")]
    public Button continueButton;
    private CanvasGroup buttonCanvasGroup;

    [Header("Voice Timing")]
    public float voiceDuration = 4f;

    [Header("Blink Settings")]
    public float blinkDuration = 3f;
    public float blinkSpeed = 0.5f;

    private StudentAvatarData currentAvatar;
    private string childName;

    void Start()
    {
        buttonCanvasGroup = continueButton.GetComponent<CanvasGroup>();

        if (buttonCanvasGroup == null)
            buttonCanvasGroup = continueButton.gameObject.AddComponent<CanvasGroup>();

        // 🔒 Desactivar botón pero mantener visible
        continueButton.interactable = false;
        buttonCanvasGroup.alpha = 0.6f;

        var connector = ActivityConnector.Instance;
        if (connector == null)
        {
            Debug.LogError("ActivityConnector no existe.");
            return;
        }

        var profile = ProfilesManager.Instance.currentProfile;
        childName = ProfilesManager.Instance.GetCurrentProfileName();
        currentAvatar = avatarDatabase.GetById(profile.avatarId);

        bool won = connector.LevelWon;

        winPanel.SetActive(won);
        losePanel.SetActive(!won);

        int hits = FeedbackManager.Instance.GetHits();
        int mistakes = FeedbackManager.Instance.GetMistakes();
        float time = FeedbackManager.Instance.GetTime();

        string text;

        if (won)
        {
            hitsTextWin.text = hits.ToString();
            mistakesTextWin.text = mistakes.ToString();
            timeTextWin.text = FormatTime(time);

            avatarImage.sprite = currentAvatar.celebrationSprite;

            text = $"¡Excelente {childName}! Lo hiciste muy bien. ¡Vamos por el siguiente nivel!";
        }
        else
        {
            hitsTextLose.text = hits.ToString();
            mistakesTextLose.text = mistakes.ToString();
            timeTextLose.text = FormatTime(time);

            avatarImage.sprite = currentAvatar.sadSprite;

            text = $"{childName}, lo intentaste. ¡Vamos a seguir practicando!";
        }

        ShowActivityMedal();

        StartCoroutine(PlayVoiceAndEnableButton(text));
    }

    IEnumerator PlayVoiceAndEnableButton(string text)
    {
        TTSManager.Instance.Speak(text);

        // Esperar tiempo fijo
        yield return new WaitForSeconds(voiceDuration);

        // Activar botón
        continueButton.interactable = true;

        // Parpadeo suave (sin desaparecer)
        yield return StartCoroutine(BlinkButton());
    }

    IEnumerator BlinkButton()
    {
        float timer = 0f;

        while (timer < blinkDuration)
        {
            buttonCanvasGroup.alpha = 1f;
            yield return new WaitForSeconds(blinkSpeed);

            buttonCanvasGroup.alpha = 0.5f;
            yield return new WaitForSeconds(blinkSpeed);

            timer += blinkSpeed * 2f;
        }

        // Dejar completamente visible al final
        buttonCanvasGroup.alpha = 1f;
    }

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