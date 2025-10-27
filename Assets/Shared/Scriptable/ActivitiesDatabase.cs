using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Music/Activities Database")]
public class ActivitiesDatabase : ScriptableObject
{
    public List<ActivityDefinition> activities;
    public ActivityDefinition GetById(string id)
    {
        return activities.Find(a => a.activityName == id);
    }
}
