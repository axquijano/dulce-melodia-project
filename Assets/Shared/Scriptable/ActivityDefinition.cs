using UnityEngine;
using System.Collections.Generic;
using System.Collections;


[CreateAssetMenu(menuName = "Music/Activity Definition")]
public class ActivityDefinition : ScriptableObject
{
    public string activityId;       // "rana", "globos", etc
    public string activityName;     // Rana Saltarina
    public Sprite icon;  
    public Sprite pressedIcon;     // Actividad seleccionada/presionada
    public Sprite unlockedIcon;    // Opcional si quieres ícono especial para desbloqueado
    public string gameplaySceneName;
    public string tutorialSceneName; // Nombre del tutorial
    public List<LevelSequence> levels;  // N niveles
    public List<LevelSettings> levelSettings; // Configuraciones por nivel
    [Header("Special Level Scenes (optional)")]
    public string thirdLevelSceneName; //para la tercera actividad
}
