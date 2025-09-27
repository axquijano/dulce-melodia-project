using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Guardado, carga, perfiles, actualizacion 

public class ProfilesManager : MonoBehaviour
{
    public static ProfilesManager Instance;
    public ChildProfile currentProfile;
    public ActivitiesDatabase databaseActividades;
    private string savePath;
    private ProfilesDatabase database = new ProfilesDatabase();


    //Patron Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Load();
    }


    public void Save()
    {
        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            database = JsonUtility.FromJson<ProfilesDatabase>(json);
        }
        else
        {
            database = new ProfilesDatabase();
            Save();
        }
    }


    public ChildProfile CreateProfile(string name)
    {
        ChildProfile profile = new ChildProfile();
        profile.childName = name;

        // Crear actividades según la base de datos
        foreach (var act in databaseActividades.activities)
        {
            int levelCount = act.levels.Count; // cantidad de niveles definida en el ScriptableObject
            profile.activities.Add(act.activityId, CreateActivity(levelCount));
        }

        database.profiles.Add(profile);
        Save();

        return profile;
    }


    private ActivityData CreateActivity(int levelCount)
    {
        ActivityData activity = new ActivityData();

        for (int i = 0; i < levelCount; i++)
            activity.levels.Add(new LevelData());

        return activity;
    }


    public void SetCurrentProfile(string name)
    {
        currentProfile = database.profiles.Find(p => p.childName == name);
    }

    public List<ChildProfile> GetAllProfiles()
    {
        return database.profiles;
    }

    public void UpdateLevelData(int levelIndex, float time, int hits, int mistakes, bool win)
    {
        var activityKey = GameFlowManager.Instance.selectedActivity.activityId;
        var profile = currentProfile;

        if(!profile.activities.ContainsKey(activityKey))
            profile.activities[activityKey] = new ActivityData();

        var activityData = profile.activities[activityKey];

        while(activityData.levels.Count <= levelIndex)
            activityData.levels.Add(new LevelData());

        var levelData = activityData.levels[levelIndex];

        if (levelData.bestTime < 0 || time < levelData.bestTime)
            levelData.bestTime = time;

        if (hits > levelData.bestHits)
            levelData.bestHits = hits;

        if (levelData.bestMistakes < 0 || mistakes < levelData.bestMistakes)
            levelData.bestMistakes = mistakes;

        levelData.retries++;
        profile.activities[activityKey].levels[levelIndex] = levelData;

        Save(); // Método que guarda tu JSON
    }



    private int CalculateStars(float time, int mistakes)
    {
        if (mistakes == 0) return 3;
        if (mistakes <= 2) return 2;
        return 1;
    }
}
