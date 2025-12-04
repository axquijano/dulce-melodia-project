using UnityEngine;
using TMPro;

public class MapLevelUI : MonoBehaviour
{
    [Header("Containers")]
    public Transform levelsContainer;

    [Header("Prefabs")]
    public GameObject levelButtonPrefab;

    private ActivityDefinition currentActivity;
    private ChildProfile profile;
    public TMP_Text text;

    void Start()
    {
        currentActivity = GameFlowManager.Instance.selectedActivity;
        text.text = "Actividad " + currentActivity.activityName;
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
            Debug.Log("MapLevel , recorriendo el level "+i);
            item.Setup(i, profile);
        }
    }
 
    public void Back()
    {
        SceneLoader.Instance.LoadScene("MapActivity");
    }
}
