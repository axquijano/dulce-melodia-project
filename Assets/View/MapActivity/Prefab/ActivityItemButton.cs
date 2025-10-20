using UnityEngine;
using UnityEngine.UI;

public class ActivityItemButton : MonoBehaviour
{
    public Button button;

    private int index;

    public void Setup(ActivityDefinition def, int i, ChildProfile profile)
    {
        index = i;

        button.image.sprite = def.unlockedIcon;

        var ss = button.spriteState;
        ss.pressedSprite = def.pressedIcon;
        button.spriteState = ss;

        button.interactable = profile.activities[i].unlocked;
    }

    public void OnClick()
    {
        GameFlowManager.Instance.SelectActivity(index);
    }
}
