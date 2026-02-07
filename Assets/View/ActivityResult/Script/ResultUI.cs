using UnityEngine;
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

    void Start()
    {
        var connector = ActivityConnector.Instance;
        if (connector == null)
        {
            Debug.LogError("ActivityConnector no existe.");
            return;
        }

        bool won = connector.LevelWon;

        winPanel.SetActive(won);
        losePanel.SetActive(!won);

        int hits = connector.Hits;
        int mistakes = connector.Mistakes;
        float time = connector.ElapsedTime;

        if (won)
        {
            hitsTextWin.text = hits.ToString();
            mistakesTextWin.text = mistakes.ToString();
            timeTextWin.text = FormatTime(time);
        }
        else
        {
            hitsTextLose.text = hits.ToString();
            mistakesTextLose.text = mistakes.ToString();
            timeTextLose.text = FormatTime(time);
        }
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
