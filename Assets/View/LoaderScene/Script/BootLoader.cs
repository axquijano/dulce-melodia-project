using UnityEngine;

public class BootLoader : MonoBehaviour
{
    void Awake()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
