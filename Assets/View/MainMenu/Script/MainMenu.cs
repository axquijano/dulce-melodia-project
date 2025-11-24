using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GoToUserSelect()
    {
        SceneLoader.Instance.LoadScene("UserSelect");
    }

    public void GoToActivityMap()
    {
        SceneLoader.Instance.LoadScene("MapActivity");
    }

}
