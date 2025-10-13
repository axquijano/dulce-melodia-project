using UnityEngine;
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    public GameObject panelGame;
    public GameObject panelPausa;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PauseGame()
    {
        panelPausa.SetActive(true);
        panelGame.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        panelPausa.SetActive(false);
        panelGame.SetActive(true);
        Time.timeScale = 1f;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("MapActivity");
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

    #if UNITY_ANDROID
        Application.Quit();
    #elif UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}
