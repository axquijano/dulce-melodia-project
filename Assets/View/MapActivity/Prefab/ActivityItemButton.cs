using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivityItemButton : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text label;
    public Image iconImage;
    public TMP_Text starsText;
    public Button button;

    private int activityIndex;
    private ActivityDefinition activityDef;

    public void Setup(ActivityDefinition def, int index, ChildProfile profile)
    {
        activityDef = def;
        activityIndex = index;

        // Texto e icono
        label.text = def.activityName;
        iconImage.sprite = def.icon;

        // ¿El niño ya jugó esta actividad?
        bool completed = index < profile.currentActivityIndex;
        bool unlocked = index == profile.currentActivityIndex;
        bool locked = index > profile.currentActivityIndex;

        // Dibujar estrellas si ya completó
       /*  if (completed)
        {
            int totalStars = CalculateActivityStars(profile, def.activityId);
            starsText.text = Stars(totalStars);
        }
        else
        {
            starsText.text = "";
        }
 */
        if (completed)
            SetCompleted();
        else if (unlocked)
            SetUnlocked();
        else
            SetLocked();
    }

    // ESTRELLAS TOTAL POR ACTIVIDAD
/*     private int CalculateActivityStars(ChildProfile p, string activityId)
    {
        if (!p.activities.ContainsKey(activityId))
            return 0;

        int total = 0;
        foreach (var lvl in p.activities[activityId].levels)
            total += lvl.stars;

        total = Mathf.Clamp(total, 0, 3); 
        return total;
    }
 */
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
        label.color = Color.white;
        iconImage.color = Color.white;
    }

    private void SetUnlocked()
    {
        button.interactable = true;
        label.color = Color.yellow;
        iconImage.color = Color.yellow;
    }

    private void SetLocked()
    {
        button.interactable = false;
        label.color = Color.gray;
        iconImage.color = Color.gray;
    }

    public void OnClick()
    {
        GameFlowManager.Instance.SelectActivity(activityIndex);
    }
}
