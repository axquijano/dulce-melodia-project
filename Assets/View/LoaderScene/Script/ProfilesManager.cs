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
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        filePath = Path.Combine(Application.persistentDataPath, "profiles.json");
        LoadProfiles();
    }

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
    }


    public void SetCurrentProfile(string name)
    {
        currentProfile = wrapper.profiles.Find(p => p.childName == name);
        SaveProfiles();
    }

    public void CreateProfile(string name, StudentAvatarData avatar)
    {
        ChildProfile profile = new ChildProfile(name, avatar.avatarName);

        for (int i = 0; i < db.activities.Count; i++)
        {
            ActivityDefinition def = db.activities[i];

            ActivityEntry entry = new ActivityEntry
            {
                key = def.activityName,
                unlocked = false,
                value = new ActivityData
                {
                    tutorialSeen = true,
                    levels = new List<LevelData>()
                }
            };

            for (int l = 0; l < def.levels.Count; l++)
                entry.value.levels.Add(new LevelData());

            profile.activities.Add(entry);
        }

        profile.activities[0].unlocked = true;

        wrapper.profiles.Add(profile);
        SaveProfiles();
    }

    public LevelData GetCurrentLevelData()
    {
        int a = PlayerPrefs.GetInt("CurrentActivity");
        int l = PlayerPrefs.GetInt("CurrentLevel");
        return currentProfile.activities[a].value.levels[l];
    }

    public void UpdateCurrentLevelData(LevelData data)
    {
        int a = PlayerPrefs.GetInt("CurrentActivity");
        int l = PlayerPrefs.GetInt("CurrentLevel");
        currentProfile.activities[a].value.levels[l] = data;
        SaveProfiles();
    }


    public bool IsActivityCompleted(ActivityEntry activity)
    {
        foreach (var lvl in activity.value.levels)
        {
            if (!lvl.CompletedAtLeastOnce())
                return false;
        }

        return !string.IsNullOrEmpty(activity.lastSelectedEmotion);
    }

    public void TryUnlockNextActivity()
    {
        for (int i = 0; i < currentProfile.activities.Count - 1; i++)
        {
            ActivityEntry current = currentProfile.activities[i];
            ActivityEntry next = currentProfile.activities[i + 1];

            if (current.unlocked && !next.unlocked && IsActivityCompleted(current))
            {
                next.unlocked = true;
                SaveProfiles();
                return;
            }
        }
    }

    public List<ChildProfile> GetAllProfiles()
    {
        return wrapper.profiles;
    }

}
