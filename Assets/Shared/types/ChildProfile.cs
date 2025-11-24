using System.Collections.Generic;
using System.Collections;

[System.Serializable]
//Cada niño tiene sus datos por actividad.
public class ChildProfile
{
    public string childName;
    public int currentActivityIndex = 0;  
    public int currentLevelIndex = 0;     
    // Actividades:
    // Musica, Memoria, Colores, Ritmo, Atencion
    public Dictionary<string, ActivityData> activities = new Dictionary<string, ActivityData>();
}
