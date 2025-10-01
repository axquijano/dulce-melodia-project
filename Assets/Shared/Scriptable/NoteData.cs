using UnityEngine;

[CreateAssetMenu(menuName = "Music/Note Data")]
public class NoteData : ScriptableObject
{
    public string noteName;  // "C", "A", etc
    public AudioClip sound;
    public Sprite imagen;
    public Color color;
}
