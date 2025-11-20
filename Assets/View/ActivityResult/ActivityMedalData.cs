using UnityEngine;

[CreateAssetMenu(
    fileName = "NewActivityMedal",
    menuName = "DulceMelodia/Activity Medal"
)]
public class ActivityMedalData : ScriptableObject
{
    public string activityKey; // activity.activityName

    [Header("Medal States")]
    public Sprite medal_1_3;
    public Sprite medal_2_3;
    public Sprite medal_3_3;
}
