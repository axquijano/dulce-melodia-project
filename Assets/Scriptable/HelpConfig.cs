using UnityEngine;

[CreateAssetMenu(fileName = "HelpConfig", menuName = "Game/Help Settings")]
public class HelpConfig : ScriptableObject
{
    [Header("Tiempo en segundos antes de mostrar la ayuda")]
    [Range(0.5f, 10f)]
    public float delayBeforeHelp = 3f;

    [Header("Activar o desactivar ayuda visual")]
    public bool helpEnabled = true;
}
