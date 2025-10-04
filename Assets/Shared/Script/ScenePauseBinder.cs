using UnityEngine;

public class ScenePauseBinder : MonoBehaviour
{
    public GameObject panelGame;
    public GameObject panelPausa;

    void Start()
    {
        PauseManager.Instance.panelGame = panelGame;
        PauseManager.Instance.panelPausa = panelPausa;

        // Asegurar que la pausa arranca desactivada
        PauseManager.Instance.ResumeGame();
    }

    public void OnPauseButtonPressed()
    {
        PauseManager.Instance.PauseGame();
    }

    public void GoToMenu()
    {
        PauseManager.Instance.GoToMenu();
    }

    public void ResumeGame()
    {
        PauseManager.Instance.ResumeGame();
    }
}