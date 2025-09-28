using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ProfilesWrapper
{
    public List<ChildProfile> profiles = new List<ChildProfile>();
}

public class ProfilesManager : MonoBehaviour
{
    public static ProfilesManager Instance;

    public ChildProfile currentProfile;

    private string filePath;
    private ProfilesWrapper wrapper = new ProfilesWrapper();

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
            wrapper = JsonUtility.FromJson<ProfilesWrapper>(json);
        }
        else
        {
            wrapper = new ProfilesWrapper();
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

    public LevelProgressData GetCurrentLevelData()
    {
        var act = currentProfile.activities[currentProfile.currentActivityIndex];
        return act.value.levels[currentProfile.currentLevelIndex];
    }

    public void UpdateCurrentLevelData(LevelProgressData data)
    {
        var act = currentProfile.activities[currentProfile.currentActivityIndex];
        act.value.levels[currentProfile.currentLevelIndex] = data;
        SaveProfiles();
    }

    public void AdvanceLevelOrActivity()
    {
        ChildProfile p = currentProfile;

        var act = p.activities[p.currentActivityIndex];
        int maxLevels = act.value.levels.Count - 1;

        if (p.currentLevelIndex < maxLevels)
        {
            p.currentLevelIndex++;
        }
        else
        {
            p.currentLevelIndex = 0;

            if (p.currentActivityIndex < p.activities.Count - 1)
                p.currentActivityIndex++;
        }

        SaveProfiles();
    }
}
