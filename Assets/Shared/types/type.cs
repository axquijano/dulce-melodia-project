using System.Collections.Generic;

// Guarda datos del desempeño del niño
[System.Serializable]
public class LevelAttempt
{
    public float time;
    public int hits;
    public int mistakes;
    public bool completed;
    public string date;
}

// Nivel: guarda todos los intentos
[System.Serializable]
public class LevelData
{
    public List<LevelAttempt> attempts = new List<LevelAttempt>();

    public int retries => attempts.Count;

    public bool CompletedAtLeastOnce()
    {
        return attempts.Exists(a => a.completed);
    }
}

// Datos por actividad
[System.Serializable]
public class ActivityData
{
    public bool tutorialSeen;
    public List<LevelData> levels = new List<LevelData>();
}

[System.Serializable]
public class ActivityEntry
{
    public string key;
    public bool unlocked; 
    public string lastSelectedEmotion = "";
    public ActivityData value;
}

[System.Serializable]
public class ChildProfile
{
    public string childName;
    public string avatarId;
    public List<ActivityEntry> activities = new List<ActivityEntry>();

    public ChildProfile(string name, string avatarId)
    {
        this.childName = name;
        this.avatarId = avatarId;
        activities = new List<ActivityEntry>();
    }
}

[System.Serializable]
public class ProfilesDatabase
{
    public List<ChildProfile> profiles = new List<ChildProfile>();
}
