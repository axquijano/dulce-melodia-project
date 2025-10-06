using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivityItemButton : MonoBehaviour
{
    [Header("UI")]
    
    public Button button;

    private int activityIndex;
    private ActivityDefinition activityDef;

    public void Setup(ActivityDefinition def, int index, ChildProfile profile)
    {
        activityDef = def;
        activityIndex = index;

        //Sprite normal
        button.image.sprite = def.unlockedIcon;
        
        // Set pressed sprite (sprite swap)
        var spriteState = button.spriteState;
        spriteState.pressedSprite = def.pressedIcon;
        button.spriteState = spriteState;
        

        // Datos del perfil
        ActivityEntry entry = profile.activities[activityIndex];

        if (AllLevelsCompleted(entry)) {
            SetCompleted();
        } else if(profile.activities[activityIndex].unlocked){
            SetUnlocked();
        }
        else {
            SetLocked();
        }
     
    }


     private bool AllLevelsCompleted(ActivityEntry entry)
    {
        foreach (var lvl in entry.value.levels)
        {
            if (!lvl.unlocked)
                return false;
        }
        return true;
    }

    private string Stars(int count)
    {
        switch (count)
        {
            case 1: return "⭐";
            case 2: return "⭐⭐";
            case 3: return "⭐⭐⭐";
            default: return "";
        }
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
        PlayerPrefs.SetInt("CurrentActivity", activityIndex);
        PlayerPrefs.SetInt("CurrentLevel", 0); 
        PlayerPrefs.Save();
        GameFlowManager.Instance.SelectActivity(activityIndex);
    }
}
