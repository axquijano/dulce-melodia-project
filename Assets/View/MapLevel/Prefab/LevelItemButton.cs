using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelItemButton : MonoBehaviour
{
    public TMP_Text levelText;
    public Button button;
    public GameObject lockIcon;

    private ActivityDefinition activityDef;
    private int levelIndex;

    public void Setup(int index, object unusedData, ActivityDefinition def)
    {
        activityDef = def;
        levelIndex = index;

        levelText.text = "Nivel " + (index + 1);

        // Obtener perfil
        var profile = ProfilesManager.Instance.currentProfile;

        int unlockedLevel = profile.currentLevelIndex;

        bool completed = index < unlockedLevel;
        bool unlocked = index == unlockedLevel;
        bool locked = index > unlockedLevel;

        if (completed)
            SetCompleted();
        else if (unlocked)
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
        GameFlowManager.Instance.SelectLevel(levelIndex);
    }
}
