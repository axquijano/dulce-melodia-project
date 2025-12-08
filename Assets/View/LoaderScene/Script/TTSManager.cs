using UnityEngine;
using TMPro;

public class TTSManager : MonoBehaviour
{
    public AndroidTTS tts;

     private static TTSManager instance;
    public static TTSManager Instance => instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void Speak(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        tts.Speak(message);
    }
}
