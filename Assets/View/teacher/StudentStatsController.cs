using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StudentStatsController : MonoBehaviour
{
    public TMP_Text studentName;

    [Header("Activity Rows")]
    public ActivityRowUI frogRow;
    public ActivityRowUI bubbleRow;
    public ActivityRowUI starsRow;

    [Header("Level Details")]
    public Transform levelDetailsContainer;
    public GameObject levelDetailPrefab;

    [Header("Scroll")]
    public ScrollRect detailScroll;

    void Start()
    {
        LoadStats();
        StartCoroutine(ScrollToTopCo());
    }

    void LoadStats()
    {
        var profile = ProfilesManager.Instance.currentProfile;
        studentName.text = profile.childName;

        bool enCursoAsignado = false;

        SetupRow(frogRow, profile.activities[0], ref enCursoAsignado);
        SetupRow(bubbleRow, profile.activities[1], ref enCursoAsignado);
        SetupRow(starsRow, profile.activities[2], ref enCursoAsignado);

        foreach (var activity in profile.activities)
        {
            if (StatsCalculator.HasAnyAttempt(activity))
            {
                var block = Instantiate(levelDetailPrefab, levelDetailsContainer);
                block.GetComponent<LevelDetailUI>().Setup(activity);
            }
        }
    }

    void SetupRow(ActivityRowUI row, ActivityEntry activity, ref bool enCursoAsignado)
    {
        string status;

        if (!activity.unlocked)
        {
            status = "Bloqueado";
        }
        else if (StatsCalculator.IsActivityCompleted(activity))
        {
            status = "Superado";
        }
        else if (!enCursoAsignado)
        {
            status = "En curso";
            enCursoAsignado = true;
        }
        else
        {
            status = "Bloqueado";
        }

        row.Setup(activity, status);
    }

    System.Collections.IEnumerator ScrollToTopCo()
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        detailScroll.verticalNormalizedPosition = 1f;
    }

    public void Back()
    {
        SceneLoader.Instance.LoadScene("StudentListScene");
    }
}
