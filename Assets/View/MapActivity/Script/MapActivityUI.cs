using UnityEngine;

public class MapActivityUI : MonoBehaviour
{
    [Header("Data")]
    public ActivitiesDatabase activitiesDatabase;

    [Header("Prefabs")]
    public GameObject activityButtonPrefab;

    [Header("Containers")]
    public Transform container;

    void Start()
    {
        ChildProfile profile = ProfilesManager.Instance.currentProfile;

        for (int i = 0; i < activitiesDatabase.activities.Count; i++)
        {
            GameObject btn = Instantiate(activityButtonPrefab, container);
            btn.GetComponent<ActivityItemButton>()
               .Setup(activitiesDatabase.activities[i], i, profile);
        }
    }

    public void Back()
    {
        SceneLoader.Instance.LoadScene("UserSelect");
    }
}
