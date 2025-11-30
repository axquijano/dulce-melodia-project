using UnityEngine;

public class MapActivityUI : MonoBehaviour
{
    [Header("Data")]
    public ActivitiesDatabase activitiesDatabase;

    [Header("Prefabs")]
    public GameObject levelButtonPrefab;

    [Header("Containers")]
    public Transform levelsContainer;

    private ChildProfile profile;

    void Start()
    {
        profile = ProfilesManager.Instance.currentProfile;
        GenerateActivityButtons();
    }

    void GenerateActivityButtons()
    {
        //Limpiar el contenedor para poner los nuevos elementos
        foreach (Transform t in levelsContainer)
            Destroy(t.gameObject);

        // Recorremos la base de datos de actividades
        for (int i = 0; i < activitiesDatabase.activities.Count; i++)
        {
            ActivityDefinition def = activitiesDatabase.activities[i];
            //Instanciar un boton para cada actividad
            GameObject btn = Instantiate(levelButtonPrefab, levelsContainer);
        
            ActivityItemButton item = btn.GetComponent<ActivityItemButton>();

            // Config bloquear/desbloquear la actividad
            item.Setup(def, i, profile);
        }
    }

    public void Back()
    {
        SceneLoader.Instance.LoadScene("UserSelect");
    }
}
