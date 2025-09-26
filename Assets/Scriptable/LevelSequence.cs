using UnityEngine;

[CreateAssetMenu(menuName = "Music/Level Sequence")]
public class LevelSequence : ScriptableObject
{
    public NoteData[] notes;  // Secuencia de notas del nivel
}