using UnityEngine;

public class PianoScene : MonoBehaviour
{
    public void Back()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
