using UnityEngine;

[CreateAssetMenu(menuName = "Music/Note Data")]
public class NoteData : ScriptableObject
{
    public string noteName;  // "DO", "RE", etc
    public Color noteColor;
    public AudioClip sound;
}
