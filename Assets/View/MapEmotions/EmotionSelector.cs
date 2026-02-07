using UnityEngine;

public class EmotionSelector : MonoBehaviour
{
    public void SelectEmotion(string emotion)
    {
        ChildProfile profile = ProfilesManager.Instance.currentProfile;
        int activityIndex = PlayerPrefs.GetInt("CurrentActivity");

        ActivityEntry activity = profile.activities[activityIndex];

       
        activity.lastSelectedEmotion = emotion;

      
        ProfilesManager.Instance.TryUnlockNextActivity();

     
        ProfilesManager.Instance.SaveProfiles();

        Debug.Log("Emoción guardada: " + emotion);

        SceneLoader.Instance.LoadScene("MapActivity");
    }
}
