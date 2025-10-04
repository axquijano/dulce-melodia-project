using UnityEngine;
using System.Collections.Generic;
using System.Collections;


[CreateAssetMenu(menuName = "Music/Activity Definition")]
public class ActivityDefinition : ScriptableObject
{
    public string activityId;       // "rana", "globos", etc
    public string activityName;     // Rana Saltarina
    public Sprite icon;
    public string gameplaySceneName;
    public List<LevelSequence> levels;  // N niveles

    public List<LevelSettings> levelSettings; // Configuraciones por nivel
}
