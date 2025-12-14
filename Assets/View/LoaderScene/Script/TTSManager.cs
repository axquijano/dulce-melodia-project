using UnityEngine;
using TMPro;

public class TTSManager : MonoBehaviour
{
    public AndroidTTS tts;

    public static TTSManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void Speak(string message)
    {
        tts.Speak(message);
    }
}
