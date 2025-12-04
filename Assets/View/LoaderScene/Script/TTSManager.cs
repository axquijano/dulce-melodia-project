using UnityEngine;
using System.Collections;

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

    /// <summary>
    /// Habla inmediatamente.
    /// </summary>
    public void Speak(string text)
    {
        if (tts != null)
            tts.Speak(text);
    }

    /// <summary>
    /// Habla después de un retardo.
    /// </summary>
    public void SpeakDelayed(string text, float delay)
    {
        StartCoroutine(DelayedSpeak(text, delay));
    }

    IEnumerator DelayedSpeak(string text, float delay)
    {
        yield return new WaitForSeconds(delay);
        Speak(text);
    }
}
