
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class ActivityThreeRecorder : MonoBehaviour
{
    [Header("Piano")]
    public PianoKey[] pianoKeys;

    [Header("Session")]
    public string fileNamePrefix = "play_session";

    private PlaySessionData sessionData;
    private float sessionTimer;
    private bool recording = false;

    // --------------------------------------------------------
    void Start()
    {
        StartRecording();
    }

    // --------------------------------------------------------
    void Update()
    {
        if (!recording) return;

        sessionTimer += Time.deltaTime;
    }

    // --------------------------------------------------------
    void StartRecording()
    {
        sessionData = new PlaySessionData
        {
            sessionStart = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
        };

        sessionTimer = 0f;
        recording = true;

        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;

        Debug.Log("🎙️ Grabación iniciada");
    }

    // --------------------------------------------------------
    void OnKeyPressed(NoteData note)
    {
        if (!recording) return;

        PlayedNoteRecord record = new PlayedNoteRecord
        {
            note = note.noteName,
            time = Mathf.Round(sessionTimer * 1000f) / 1000f
        };

        sessionData.notes.Add(record);

        Debug.Log($"🎵 {record.note} @ {record.time}s");
    }

    // --------------------------------------------------------
    public void StopAndSave()
    {
        recording = false;

        foreach (var key in pianoKeys)
            key.onKeyPressed -= OnKeyPressed;

        SaveToJson();
    }

    // --------------------------------------------------------
    void SaveToJson()
    {
        string json = JsonUtility.ToJson(sessionData, true);

#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(
            AndroidExternalStoragePath(),
            $"{fileNamePrefix}_{sessionData.sessionStart}.json"
        );
#else
        string path = Path.Combine(
            Application.persistentDataPath,
            $"{fileNamePrefix}_{sessionData.sessionStart}.json"
        );
#endif

        File.WriteAllText(path, json);

        Debug.Log($"💾 Archivo guardado en:\n{path}");
    }

    // --------------------------------------------------------
    string AndroidExternalStoragePath()
    {
        return "/storage/emulated/0/Documents";
    }
}
