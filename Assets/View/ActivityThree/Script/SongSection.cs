using UnityEngine;

[System.Serializable]
public class TimedNote
{
    public NoteData note;
    public float time;   // segundo exacto en el audio
    public bool isGhost;
    public AudioClip overrideSound; // grave / agudo / especial (opcional)

}

[System.Serializable]
public class SongSection
{
    public AudioClip referenceAudio;
    public TimedNote[] row1;  
    public TimedNote[] row2;
}
