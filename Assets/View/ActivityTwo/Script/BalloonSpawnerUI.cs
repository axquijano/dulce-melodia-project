using UnityEngine;

public class BalloonSpawnerUI : MonoBehaviour
{
    [Header("References")]
    public BalloonControllerUI balloonPrefab;
    public RectTransform spawnPanel;

    public BalloonControllerUI SpawnBalloon()
    {
        BalloonControllerUI balloon = Instantiate(balloonPrefab, spawnPanel);

        RectTransform rect = balloon.GetComponent<RectTransform>();

        float halfWidth = spawnPanel.rect.width * 0.5f;
        float x = Random.Range(-halfWidth, halfWidth);

        float startY = -spawnPanel.rect.height * 0.5f - 50f;

        rect.anchoredPosition = new Vector2(x, startY);

        return balloon;
    }
}
