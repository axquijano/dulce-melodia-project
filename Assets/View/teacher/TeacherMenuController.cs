using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherMenuController : MonoBehaviour
{
    // Cargar escena para crear estudiante
    public void GoToCreateStudent()
    {
        SceneManager.LoadScene("CreateStudentScene");
    }

    // Cargar escena para ver progresos
    public void GoToViewProgress()
    {
        SceneManager.LoadScene("ProgressScene");
    }

    // (Opcional) volver al menú principal
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
