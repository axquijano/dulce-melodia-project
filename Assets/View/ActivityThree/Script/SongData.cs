using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "SongData",
    menuName = "Music/Song Data",
    order = 1
)]
public class SongData : ScriptableObject
{
    public List<SongSection> sections = new();
}
