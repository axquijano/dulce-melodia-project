using System.Collections.Generic;
using System.Collections;

[System.Serializable]
//Guarda datos del desempeño del niño
public class LevelData
{
    public float bestTime = -1f;
    public int bestHits = -1;
    public int bestMistakes = -1;
    //TODO : se cambio
    public bool unlocked = true;

    public int retries = 0;
    public int stars = 0; // 0–3
}



[System.Serializable]
//Cantidad de niveles variable por actividad
public class ActivityData
{
    public bool tutorialSeen;  // Si ya vio el tutorial
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
//Cada niño tiene sus datos por actividad.
public class ChildProfile
{
    public string childName;   
    // Actividades:
    // Musica, Memoria, Colores, Ritmo, Atencion
    public List<ActivityEntry> activities = new List<ActivityEntry>();
    
    public ChildProfile(string name)
    {
        childName = name;
        activities = new List<ActivityEntry>();
    }
}


[System.Serializable]
//Almacena todos los perfiles en el archivo JSON.
public class ProfilesDatabase
{
    public List<ChildProfile> profiles = new List<ChildProfile>();
}
