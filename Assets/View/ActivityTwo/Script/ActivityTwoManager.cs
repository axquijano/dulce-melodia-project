using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class ActivityTwoManager : MonoBehaviour
{

    public LevelSettings settings;
    public NoteData[] notes;
    public PianoKey[] pianoKeys;

    [Header("References")]
    public BalloonSpawnerUI spawner;

    private float timer;
    private int hits;
    private int mistakes;

    private List<BalloonControllerUI> activeBalloons = new List<BalloonControllerUI>();
    public string childName;

    void Start()
    {
        ActivityConnector.Instance.StartLevel();
        childName = ProfilesManager.Instance.currentProfile.childName;
        settings = GameFlowManager.Instance.GetCurrentLevelSettings();
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
        StartCoroutine(welcome());

    }

    IEnumerator welcome()
    {
        if (GameFlowManager.Instance.GetLevel() == 0)
        {
            yield return new WaitForSeconds(0.3f);

            string message =
                childName + ". " +
                "Estalla los globos. " +
                "Toca la nota dentro del globo.";

            TTSManager.Instance.Speak(message);
        }
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

        // ⚡ Velocidad del LevelSettings
        balloon.SetSpeed(settings.balloonSpeed);

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
   /*  public void RegisterBalloonHit(NoteData note)
    {
        hits++;
        FeedbackManager.Instance.RegisterHit();

        if (hits >= settings.balloonsToWin)
            Debug.Log("🏆 GANASTE");

        RemoveBalloon(note);
    } */

    public void RegisterBalloonHit(BalloonControllerUI balloon)
    {
        if (balloon == null) return;

        hits++;
        FeedbackManager.Instance.RegisterHit();
        ActivityConnector.Instance.RegisterHit();

        if (hits >= settings.balloonsToWin){
            ActivityConnector.Instance.OnWin(); 
            return;
        }
            /* Debug.Log("🏆 GANASTE"); */

        RemoveBalloon(balloon);
    }


    // -------------------------
    // GLOBO QUE SE ESCAPA
    // -------------------------
    /* public void RegisterBalloonMiss(NoteData note)
    {
        mistakes++;
        FeedbackManager.Instance.RegisterMistake();
        if (mistakes >= settings.allowedMistakes)
            Debug.Log("❌ PERDISTE");

        RemoveBalloon(note);
    } */

   /*  public void RegisterBalloonMiss(NoteData note)
    {
        mistakes++;
        FeedbackManager.Instance.RegisterMistake();

        // Evita que destruya globos que ya no existen
        if (!activeBalloons.Exists(b => b != null && b.noteData == note))
            return;

        if (mistakes >= settings.allowedMistakes)
            Debug.Log("❌ PERDISTE");

        RemoveBalloon(note);
    } */

public void RegisterBalloonMiss(BalloonControllerUI balloon)
{
    if (balloon == null) return;

    mistakes++;
    FeedbackManager.Instance.RegisterMistake();
    ActivityConnector.Instance.RegisterMistake();

    if (mistakes >= settings.allowedMistakes)
        {
            ActivityConnector.Instance.OnLose(); 
            return;
        }
       /*  Debug.Log("❌ PERDISTE"); */

    RemoveBalloon(balloon);
}


   /*  void RemoveBalloon(NoteData note)
    {
        // Buscar el primer globo que coincida
        var balloon = activeBalloons.Find(b => b != null && b.noteData == note);

        if (balloon != null)
        {
            if (balloon != null && balloon.gameObject != null)
                balloon.Pop();

            activeBalloons.Remove(balloon);
        }

    } */

    void RemoveBalloon(BalloonControllerUI balloon)
    {
        if (balloon == null) return;

        if (activeBalloons.Contains(balloon))
            activeBalloons.Remove(balloon);

        balloon.Pop();
    }



    // -------------------------
    // TECLA PIANO PRESIONADA
    // -------------------------
    /* void OnKeyPressed(NoteData pressedNote)
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
    } */
    void OnKeyPressed(NoteData pressedNote)
    {
        // Busca el PRIMER globo que salió con esa nota
        var balloon = activeBalloons.Find(b => b.noteData == pressedNote);

        if (balloon != null)
        {
            RegisterBalloonHit(balloon);
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
