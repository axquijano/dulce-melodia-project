using UnityEngine;

[CreateAssetMenu(menuName = "Music/Level Sequence")]
public class LevelSequence : ScriptableObject
{
    public LevelNoteItem[] notes;  // Secuencia de notas del nivel
    public float allowedMistakes;
}