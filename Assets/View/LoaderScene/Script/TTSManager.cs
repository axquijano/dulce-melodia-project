using UnityEngine;

public class TTSManager : MonoBehaviour
{
    public AndroidTTS tts;
    public static TTSManager Instance;

    public bool IsSpeaking => tts != null && tts.IsSpeaking;

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

    public void Stop()
    {
        if (tts != null)
            tts.Stop();
    }

}
