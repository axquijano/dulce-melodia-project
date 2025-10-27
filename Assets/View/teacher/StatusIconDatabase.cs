using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "UI/Status Icon Database")]
public class StatusIconDatabase : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string status; // Superado, En curso, Bloqueado
        public Sprite icon;
    }

    public List<Entry> icons;

    public Sprite GetIcon(string status)
    {
        var e = icons.Find(i => i.status == status);
        return e != null ? e.icon : null;
    }
}
