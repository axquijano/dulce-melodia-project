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
        var act = currentProfile.activities[currentProfile.currentActivityIndex];
        return act.value.levels[currentProfile.currentLevelIndex];
    }

    public void UpdateCurrentLevelData(LevelData data)
    {
        var act = currentProfile.activities[currentProfile.currentActivityIndex];
        act.value.levels[currentProfile.currentLevelIndex] = data;
        SaveProfiles();
    }

    public void AdvanceLevelOrActivity()
    {
        ChildProfile p = currentProfile;

        ActivityEntry currentActivity = p.activities[p.currentActivityIndex];

        // Actividad definida en el database (para contar niveles)
        ActivityDefinition dbActivity = db.activities[p.currentActivityIndex];

        int maxLevels = dbActivity.levels.Count - 1;  // index del último nivel
        LevelData currentLevel = currentActivity.value.levels[p.currentLevelIndex];

        // ----------------------------
        // 1. SI HAY SIGUIENTE NIVEL
        // ----------------------------
        if (p.currentLevelIndex < maxLevels)
        {
            // Desbloquear SIGUIENTE nivel
            int nextLevel = p.currentLevelIndex + 1;
            currentActivity.value.levels[nextLevel].unlocked = true;

            // Avanza al siguiente nivel
            p.currentLevelIndex = nextLevel;

            SaveProfiles();
            return;
        }

        // -------------------------------------
        // 2. SI NO HAY MÁS NIVELES → ACTIVIDAD
        // -------------------------------------

        // Reiniciar level index para la nueva actividad
        p.currentLevelIndex = 0;

        // Si se puede avanzar a otra actividad
        if (p.currentActivityIndex < p.activities.Count - 1)
        {
            int nextActivity = p.currentActivityIndex + 1;

            // Desbloquear la siguiente actividad
            p.activities[nextActivity].unlocked = true;

            // Desbloquear su primer nivel
            p.activities[nextActivity].value.levels[0].unlocked = true;

            // Cambiar actividad
            p.currentActivityIndex = nextActivity;

            SaveProfiles();
            return;
        }

        // -------------------------------------
        // 3. NO HAY MÁS ACTIVIDADES
        // -------------------------------------
        // Simplemente queda en la última actividad / nivel
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
            entry.unlocked = false;
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
