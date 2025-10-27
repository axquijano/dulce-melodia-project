using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GoToUserSelect()
    {
        SceneLoader.Instance.LoadScene("UserSelect");
    }

    public void GoToTeacherMenu(){
        SceneLoader.Instance.LoadScene("TeacherMenu");
    }

    public void ExitGame()
    {
        #if UNITY_ANDROID
            Application.Quit();
        #elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
