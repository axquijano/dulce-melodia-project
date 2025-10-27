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
        SceneManager.LoadScene("StudentListScene");
    }

    //volver al menú principal
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Borra todos los datos 
    public void ClearAll()
    {
        ProfilesManager.Instance.ClearAllData();
    }
}
