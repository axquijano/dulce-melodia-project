using UnityEngine;

public class EmotionSelector : MonoBehaviour
{
    public void SelectEmotion(string emotion)
    {
        // Guardar emoción en el perfil actual
        
        ChildProfile profile = ProfilesManager.Instance.currentProfile;
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");
        profile.activities[activityIndex].lastSelectedEmotion = emotion;

        // Guardar en el JSON
        ProfilesManager.Instance.SaveProfiles();

        Debug.Log("Emoción guardada: " + emotion);

        // Volver al mapa de actividades
        SceneLoader.Instance.LoadScene("MapActivity");
    }
}
