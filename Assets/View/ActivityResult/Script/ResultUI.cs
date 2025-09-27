using UnityEngine;

public class ResultUI : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;

    void Start()
    {
        if (ActivityConnector.Instance.levelWon)
        {
            winPanel.SetActive(true);
            losePanel.SetActive(false);
        }
        else
        {
            losePanel.SetActive(true);
            winPanel.SetActive(false);
        }
    }

    public void BackToMap()
    {
        ActivityConnector.Instance.BackToMap();
    }
}
