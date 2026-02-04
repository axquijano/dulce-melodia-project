using UnityEngine;
using System.Collections.Generic;

public class ActivityTwoManager : MonoBehaviour
{
    [Header("Config")]
    public NoteData[] notes;
    public PianoKey[] pianoKeys;

    [Header("UI")]
    public BalloonSpawnerUI spawner;

    private LevelSettings settings;
    private float spawnTimer;

    private readonly List<BalloonControllerUI> activeBalloons = new();

    void Start()
    {
        ActivityConnector.Instance.StartLevel();

        LoadLevelSettings();
        LinkPianoKeys();
    }

    void LoadLevelSettings()
    {
        ActivityDefinition activity = GameFlowManager.Instance.selectedActivity;
        int levelIndex = GameFlowManager.Instance.selectedLevel;

        settings = activity.levelSettings[levelIndex];

        FeedbackManager.Instance.SetMaxMistakes(settings.allowedMistakes);
    }

    void LinkPianoKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= settings.spawnInterval)
        {
            spawnTimer = 0f;
            SpawnBalloon();
        }
    }

    // SPAWN
    void SpawnBalloon()
    {
        var balloon = spawner.SpawnBalloon();
        balloon.SetSpeed(settings.balloonSpeed);

        NoteData note = notes[Random.Range(0, notes.Length)];

        float r = Random.value;

        bool full = r <= settings.pctFull;
        bool onlyColor = r > settings.pctFull && r <= settings.pctFull + settings.pctColor;
        bool onlyLetter = r > settings.pctFull + settings.pctColor;

        balloon.Setup(
            note,
            displayColor: full || onlyColor,
            displayLetter: full || onlyLetter,
            isBlack: false
        );

        activeBalloons.Add(balloon);
    }

    // INPUT
    void OnKeyPressed(NoteData pressedNote)
    {
        var balloon = activeBalloons.Find(b => b != null && b.noteData == pressedNote);

        if (balloon != null)
        {
            RegisterHit(balloon);
        }
        else
        {
            RegisterMistake();
        }
    }

    // HIT / MISS
    void RegisterHit(BalloonControllerUI balloon)
    {
        ActivityConnector.Instance.RegisterHit();
        FeedbackManager.Instance.RegisterHit();

        RemoveBalloon(balloon);

        if (ActivityConnector.Instance.Hits >= settings.balloonsToWin)
        {
            ActivityConnector.Instance.OnWin();
        }
    }

    public void RegisterBalloonMiss(BalloonControllerUI balloon)
    {
        if (balloon == null) return;

        RegisterMistake();
        RemoveBalloon(balloon);
    }

    void RegisterMistake()
    {
        ActivityConnector.Instance.RegisterMistake();
        FeedbackManager.Instance.RegisterMistake();

        CheckLoseCondition();
    }

    void CheckLoseCondition()
    {
        if (ActivityConnector.Instance.Mistakes >= settings.allowedMistakes)
        {
            ActivityConnector.Instance.OnLose();
        }
    }
    
    // CLEANUP
    void RemoveBalloon(BalloonControllerUI balloon)
    {
        if (balloon == null) return;

        activeBalloons.Remove(balloon);
        balloon.Pop();
    }
}
