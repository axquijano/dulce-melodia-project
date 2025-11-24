using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Music/Activities Database")]
public class ActivitiesDatabase : ScriptableObject
{
    public List<ActivityDefinition> activities;
}
