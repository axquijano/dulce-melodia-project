using UnityEngine;

public class EmotionSelector : MonoBehaviour
{
    public void SelectEmotion(string emotion)
    {
        ChildProfile profile = ProfilesManager.Instance.currentProfile;
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");

        ActivityEntry activity = profile.activities[activityIndex];

        // 1️⃣ Guardar emoción
        activity.lastSelectedEmotion = emotion;

        // 2️⃣ Intentar desbloquear la siguiente actividad
        ProfilesManager.Instance.TryUnlockNextActivity();

        // 3️⃣ Guardar cambios
        ProfilesManager.Instance.SaveProfiles();

        Debug.Log("Emoción guardada: " + emotion);

        // 4️⃣ Volver al mapa de actividades
        SceneLoader.Instance.LoadScene("MapActivity");
    }
}
