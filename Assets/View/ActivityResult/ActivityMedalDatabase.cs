using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "ActivityMedalDatabase",
    menuName = "DulceMelodia/Activity Medal Database"
)]
public class ActivityMedalDatabase : ScriptableObject
{
    public List<ActivityMedalData> medals;

    public ActivityMedalData GetByActivityKey(string key)
    {
        return medals.Find(m => m.activityKey == key);
    }
}
