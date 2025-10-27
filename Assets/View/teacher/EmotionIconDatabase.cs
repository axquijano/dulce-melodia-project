using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "UI/Emotion Icon Database")]
public class EmotionIconDatabase : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string emotion; // feliz, normal, triste
        public Sprite icon;
    }

    public List<Entry> icons;

    public Sprite GetIcon(string emotion)
    {
        var e = icons.Find(i => i.emotion.ToLower() == emotion.ToLower());
        return e != null ? e.icon : null;
    }
}
