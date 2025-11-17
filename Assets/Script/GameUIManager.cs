using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("Panels")]
    public GameObject gamePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowWin()
    {
        gamePanel.SetActive(false);
        winPanel.SetActive(true);
        losePanel.SetActive(false);
    }

    public void ShowLose()
    {
        gamePanel.SetActive(false);
        losePanel.SetActive(true);
        winPanel.SetActive(false);
    }

    public void BackToGame()
    {
        gamePanel.SetActive(true);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }
}
