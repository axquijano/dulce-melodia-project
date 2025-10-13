using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayedNoteRecord
{
    public string note;
    public float time;
}

[System.Serializable]
public class PlaySessionData
{
    public string sessionStart;
    public List<PlayedNoteRecord> notes = new();
}

