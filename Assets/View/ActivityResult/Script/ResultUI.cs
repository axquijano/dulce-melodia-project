using UnityEngine;

public class ResultUI : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
   /*  public TMPro.TMP_Text hitsText;
    public TMPro.TMP_Text mistakesText;
    public TMPro.TMP_Text timeText; */

    void Start()
    {
        if (ActivityConnector.Instance.LevelWon)
        {
            winPanel.SetActive(true);
            losePanel.SetActive(false);
        }
        else
        {
            losePanel.SetActive(true);
            winPanel.SetActive(false);
        }

      /*   hitsText.text = ActivityConnector.Instance.hits.ToString();
        mistakesText.text = ActivityConnector.Instance.mistakes.ToString();
        timeText.text = ActivityConnector.Instance.time.ToString("F2"); */
    }

    public void BackToMap()
    {
        ActivityConnector.Instance.BackToMap();
    }
}
