using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelItemButton : MonoBehaviour
{
    public TMP_Text levelText;
    public Button button;
    public GameObject lockIcon;
    private int levelIndex;

    public void Setup(int index, ChildProfile profile)
    {
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");
        levelIndex = index;

        levelText.text = "Nivel " + (index + 1);

        // Obtener perfil
        bool  unlockedLevel = profile.activities[activityIndex].value.levels[levelIndex].unlocked;

       if (unlockedLevel)
            SetUnlocked();
        else
            SetLocked();
    }

    private void SetCompleted()
    {
        button.interactable = true;
        levelText.color = Color.white;

        if (lockIcon != null)
            lockIcon.SetActive(false);
    }

    private void SetUnlocked()
    {
        button.interactable = true;
        levelText.color = Color.yellow;

        if (lockIcon != null)
            lockIcon.SetActive(false);
    }

    private void SetLocked()
    {
        button.interactable = false;
        levelText.color = Color.gray;

        if (lockIcon != null)
            lockIcon.SetActive(true);
    }

    public void OnClick()
    {
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();

        GameFlowManager.Instance.selectedLevel = levelIndex;
        SceneLoader.Instance.LoadScene(GameFlowManager.Instance.selectedActivity.gameplaySceneName);
    }

}
