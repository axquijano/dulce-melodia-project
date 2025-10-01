using UnityEngine;

public class MapLevelUI : MonoBehaviour
{
    [Header("Containers")]
    public Transform levelsContainer;

    [Header("Prefabs")]
    public GameObject levelButtonPrefab;

    private ActivityDefinition currentActivity;
    private ChildProfile profile;

    void Start()
    {
        currentActivity = GameFlowManager.Instance.selectedActivity;
        profile = ProfilesManager.Instance.currentProfile;

        Setup(currentActivity);
    }

    public void Setup(ActivityDefinition def)
    {
        currentActivity = def;
        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        foreach (Transform t in levelsContainer)
            Destroy(t.gameObject);

        // 🔥 Recorrer niveles reales del ActivityDefinition
        for (int i = 0; i < currentActivity.levels.Count; i++)
        {
            GameObject btn = Instantiate(levelButtonPrefab, levelsContainer);
            LevelItemButton item = btn.GetComponent<LevelItemButton>();

            // Como LevelSequence NO tiene datos extra, usamos null
            item.Setup(i, profile);
        }
    }
 
    public void Back()
    {
        SceneLoader.Instance.LoadScene("MapActivity");
    }
}
