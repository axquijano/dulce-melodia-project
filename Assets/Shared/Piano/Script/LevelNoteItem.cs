using UnityEngine;

[System.Serializable]
public class LevelNoteItem
{
    public NoteData note;          // Nota original (C, D, E...)
    public bool showColor = true;  // Mostrar color
    public bool showLetter = true; // Mostrar letra
}
