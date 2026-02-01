using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProfilesManager : MonoBehaviour
{
    public static ProfilesManager Instance;

    public ChildProfile currentProfile;
    public ActivitiesDatabase db;
    private string filePath;
    private ProfilesDatabase wrapper = new ProfilesDatabase();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            filePath = Path.Combine(Application.persistentDataPath, "profiles.json");
            LoadProfiles();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        PlayerPrefs.SetInt("CurrentActivity", 0);
        PlayerPrefs.SetInt("CurrentLevel", 0); 
        PlayerPrefs.Save();
    }

    // ---------------------------
    // GETTERS NECESARIOS
    // ---------------------------

    public List<ChildProfile> GetAllProfiles()
    {
        return wrapper.profiles;
    }

    public void SetCurrentProfile(string name)
    {
        currentProfile = wrapper.profiles.Find(p => p.childName == name);
        SaveProfiles();
    }

    // ---------------------------
    // LOADING / SAVING
    // ---------------------------

    public void LoadProfiles()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
             // Si está vacío o inválido → crear uno nuevo
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("ProfilesManager: JSON vacío. Creando nuevo archivo.");
                wrapper = new ProfilesDatabase();
                wrapper.profiles = new List<ChildProfile>();
                SaveProfiles();
                return;
            }

            wrapper = JsonUtility.FromJson<ProfilesDatabase>(json);

        }
        else
        {
            wrapper = new ProfilesDatabase();
            SaveProfiles();
        }
    }

    public void SaveProfiles()
    {
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Profiles saved!");
    }

    // ---------------------------
    // RETORNAR Y MODIFICAR DATOS
    // ---------------------------

    public LevelData GetCurrentLevelData()
    {
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");
        int levelIndex = PlayerPrefs.GetInt("CurrentLevel");
        var act = currentProfile.activities[activityIndex];
        return act.value.levels[levelIndex];
    }

    public void UpdateCurrentLevelData(LevelData data)
    {
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");
        int levelIndex = PlayerPrefs.GetInt("CurrentLevel");
        var act = currentProfile.activities[activityIndex];
        act.value.levels[levelIndex] = data;
        SaveProfiles();
    }

   public void AdvanceLevelOrActivity()
{
    ChildProfile p = currentProfile;

    // -----------------------------
    // Leer índices desde PlayerPrefs
    // -----------------------------
    int activityIndex = PlayerPrefs.GetInt("CurrentActivity");
    int levelIndex = PlayerPrefs.GetInt("CurrentLevel");

    ActivityEntry currentActivity = p.activities[activityIndex];
    ActivityDefinition dbActivity = db.activities[activityIndex];

    int maxLevels = dbActivity.levels.Count - 1;

    // ----------------------------
    // 1. SI HAY SIGUIENTE NIVEL
    // ----------------------------
    if (levelIndex < maxLevels)
    {
        int nextLevel = levelIndex + 1;

        // Desbloquear siguiente nivel
        currentActivity.value.levels[nextLevel].unlocked = true;

        // Guardar el nuevo level en PlayerPrefs
       /*  PlayerPrefs.SetInt("CurrentLevel", nextLevel); */
        PlayerPrefs.Save();

        SaveProfiles();
        return;
    }

    // -------------------------------------
    // 2. SI NO HAY MÁS NIVELES → ACTIVIDAD
    // -------------------------------------

    // Resetea el nivel para la siguiente actividad
    /* PlayerPrefs.SetInt("CurrentLevel", 0); */

    // Si se puede avanzar a otra actividad
    if (activityIndex < p.activities.Count - 1)
    {
        int nextActivity = activityIndex + 1;

        // Desbloquear actividad
        p.activities[nextActivity].unlocked = true;

        // Desbloquear primer nivel de esa actividad
        p.activities[nextActivity].value.levels[0].unlocked = true;

        // Guardar nuevo activity en PlayerPrefs
        /* PlayerPrefs.SetInt("CurrentActivity", nextActivity); */
        PlayerPrefs.Save();

        SaveProfiles();
        return;
    }

    // -------------------------------------
    // 3. NO HAY MÁS ACTIVIDADES
    // -------------------------------------
    // No cambia nada, solo guardamos
    SaveProfiles();
}



    public void CreateProfile(string name)
    {
        if (db == null || db.activities == null || db.activities.Count == 0)
        {
            Debug.LogError("[ProfilesManager] No hay actividades en la base de datos.");
            return;
        }

        ChildProfile profile = new ChildProfile(name);

        foreach (var act in db.activities)
        {
            ActivityEntry entry = new ActivityEntry();
            entry.key = act.activityName;
            entry.value = new ActivityData();
            //TODO : Cambiar unlocked y tutorialSeen
            entry.unlocked = true;
            entry.value.tutorialSeen = true;
            entry.value.levels = new List<LevelData>();

            // Crear LevelData en blanco por cada nivel
            for (int i = 0; i < act.levels.Count; i++)
            {
                entry.value.levels.Add(new LevelData());
            }

            profile.activities.Add(entry);
        }

        profile.activities[0].unlocked = true;
        profile.activities[0].value.levels[0].unlocked = true;

        wrapper.profiles.Add(profile);
        SaveProfiles();

        Debug.Log("Perfil creado correctamente con todas las actividades y niveles.");
    }


}
