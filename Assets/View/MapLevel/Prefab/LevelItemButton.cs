using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelItemButton : MonoBehaviour
{
    public Button button;
    private int levelIndex;
    
    public void Setup(int index, ChildProfile profile, Sprite spriteLevel)
    { 
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");
        levelIndex = index;

        if (spriteLevel != null)
            button.image.sprite = spriteLevel;


        // Obtener perfil
        bool  unlockedLevel = profile.activities[activityIndex].value.levels[levelIndex].unlocked;
        Debug.Log("En levelItemButton level "+ index + "desbloqueado "+unlockedLevel );
       if (unlockedLevel)
            SetUnlocked();
        else
            SetLocked();
    }

    private void SetCompleted()
    {
        button.interactable = true;
    }

    private void SetUnlocked()
    {
        button.interactable = true;
    }

    private void SetLocked()
    {
        button.interactable = false;
    }

    public void OnClick()
    {
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();
        GameFlowManager.Instance.selectedLevel = levelIndex;
        int actividadIndex = PlayerPrefs.GetInt("CurrentActivity");
        Debug.Log("actividad "+actividadIndex+" nivel "+levelIndex);
        if(actividadIndex == 2 && levelIndex == 2){
            SceneLoader.Instance.LoadScene("ActivityThree_Music");
            return;
        }
        SceneLoader.Instance.LoadScene(GameFlowManager.Instance.selectedActivity.gameplaySceneName);
    }

}
