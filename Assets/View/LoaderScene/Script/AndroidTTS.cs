using UnityEngine;

public class AndroidTTS : MonoBehaviour
{
    private AndroidJavaObject ttsObject;
    private bool initialized = false;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        InitTTS();
#else
        initialized = true; // Para evitar errores en el editor
#endif
    }

    public void Speak(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!initialized) return;
        ttsObject.Call<int>("speak", message, 0, null, null);
#else
        Debug.Log("[TTS] (Editor) Speak: " + message);
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    void InitTTS()
    {
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        ttsObject = new AndroidJavaObject(
            "android.speech.tts.TextToSpeech",
            activity,
            new TTSListener(this)
        );
    }

    private void OnTTSReady()
    {
        initialized = true;
    }

    private class TTSListener : AndroidJavaProxy
    {
        private readonly AndroidTTS parent;

        public TTSListener(AndroidTTS parent)
            : base("android.speech.tts.TextToSpeech$OnInitListener")
        {
            this.parent = parent;
        }

        public void onInit(int status)
        {
            parent.SendMessage("OnTTSReady");
        }
    }
#endif
}
