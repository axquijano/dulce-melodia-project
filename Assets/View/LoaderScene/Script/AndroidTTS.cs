using UnityEngine;
using System;

public class AndroidTTS : MonoBehaviour
{
    private AndroidJavaObject ttsObject;
    private bool initialized = false;

    public bool IsSpeaking { get; private set; }

#if UNITY_ANDROID && !UNITY_EDITOR
    private string currentUtteranceId;
#endif

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        InitTTS();
#else
        initialized = true;
#endif
    }

    public void Speak(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!initialized) return;

        IsSpeaking = true;

        currentUtteranceId = Guid.NewGuid().ToString();

        // ✅ USAR BUNDLE (NO HashMap)
        AndroidJavaObject bundle =
            new AndroidJavaObject("android.os.Bundle");
        bundle.Call("putString",
            "utteranceId",
            currentUtteranceId);

        // ✅ FIRMA CORRECTA API 21+
        ttsObject.Call<int>(
            "speak",
            message,
            0,
            bundle,
            currentUtteranceId
        );
#else
        Debug.Log("[TTS] " + message);
        IsSpeaking = false;
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    void InitTTS()
    {
        var unityPlayer =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        ttsObject = new AndroidJavaObject(
            "android.speech.tts.TextToSpeech",
            activity,
            new TTSListener(this)
        );
    }

    private void OnTTSReady()
    {
        initialized = true;

        ttsObject.Call(
            "setOnUtteranceProgressListener",
            new TTSProgressListener(this)
        );
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

    private class TTSProgressListener : AndroidJavaProxy
    {
        private readonly AndroidTTS parent;

        public TTSProgressListener(AndroidTTS parent)
            : base("android.speech.tts.UtteranceProgressListener")
        {
            this.parent = parent;
        }

        public void onStart(string utteranceId)
        {
            parent.IsSpeaking = true;
        }

        public void onDone(string utteranceId)
        {
            parent.IsSpeaking = false; // ✅ CLAVE
        }

        public void onError(string utteranceId)
        {
            parent.IsSpeaking = false;
        }
    }
#endif
}
