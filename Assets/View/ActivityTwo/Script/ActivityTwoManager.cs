using UnityEngine;
using System.Collections.Generic;

public class ActivityTwoManager : MonoBehaviour
{
    public static ActivityTwoManager Instance;

    public LevelSettings settings;
    public NoteData[] notes;
    public PianoKey[] pianoKeys;

    [Header("References")]
    public BalloonSpawnerUI spawner;

    private float timer;
    private int hits;
    private int mistakes;

    private List<BalloonControllerUI> activeBalloons = new List<BalloonControllerUI>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= settings.spawnInterval)
        {
            timer = 0;
            SpawnLogic();
        }
    }

    // -------------------------
    // SPAWN DE GLOBOS
    // -------------------------
    void SpawnLogic()
    {
        var balloon = spawner.SpawnBalloon();

        NoteData randomNote = notes[Random.Range(0, notes.Length)];

        // Determinar tipo de globo según probabilidad
        float r = Random.value;

        bool full = r <= settings.pctFull;
        bool onlyColor = r > settings.pctFull && r <= settings.pctFull + settings.pctColor;
        bool onlyLetter = r > settings.pctFull + settings.pctColor;

        balloon.Setup(
            randomNote,
            displayColor: full || onlyColor,
            displayLetter: full || onlyLetter,
            isBlack: false
        );

        activeBalloons.Add(balloon);
    }

    // -------------------------
    // GOLPE A UN GLOBO
    // -------------------------
    public void RegisterBalloonHit(NoteData note)
    {
        hits++;
        FeedbackManager.Instance.RegisterHit();

        if (hits >= settings.balloonsToWin)
            Debug.Log("🏆 GANASTE");

        RemoveBalloon(note);
    }

    // -------------------------
    // GLOBO QUE SE ESCAPA
    // -------------------------
    public void RegisterBalloonMiss(NoteData note)
    {
        mistakes++;
        FeedbackManager.Instance.RegisterMistake();
        if (mistakes >= settings.allowedMistakes)
            Debug.Log("❌ PERDISTE");

        RemoveBalloon(note);
    }

    void RemoveBalloon(NoteData note)
    {
        activeBalloons.RemoveAll(b => b == null || b.noteData == note);
    }

    // -------------------------
    // TECLA PIANO PRESIONADA
    // -------------------------
    void OnKeyPressed(NoteData pressedNote)
    {
        bool exists = activeBalloons.Exists(b => b.noteData == pressedNote);

        if (exists)
        {
            RegisterBalloonHit(pressedNote);
        }
        else
        {
            mistakes++;
            Debug.Log("❌ ERROR");

            if (mistakes >= settings.allowedMistakes)
                Debug.Log("❌ PERDISTE");
        }
    }
}
