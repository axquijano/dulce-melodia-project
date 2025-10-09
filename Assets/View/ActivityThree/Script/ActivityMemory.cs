using UnityEngine;
using System.Collections;

public class ActivityMemory : MonoBehaviour
{
    [Header("UI")]
    public Transform starContainer;
    public GameObject starPrefab;

    [Header("Input")]
    public PianoKey[] pianoKeys;

    [Header("Song")]
    public SongSection[] sections;

    [Header("Audio")]
    public AudioSource audioSource;

    int currentSection = 0;
    int currentRepeat = 0;
    int playerNoteIndex = 0;

    bool waitingForInput = false;

    enum Level1Phase { None, AutoPlay, PlayerRepeat }
    Level1Phase phase = Level1Phase.None;

    void Start()
    {
        ActivityConnector.Instance.StartLevel();
        LinkKeys();

        TTSManager.Instance.Speak("Las estrellas nos van a ayudar a tocar una canción.");
        StartCoroutine(StartSection());
    }

    // ---------------------------------------------------------
    // GENERAR ESTRELLAS (DOS FILAS REALES)
    // ---------------------------------------------------------
    void GenerateStarsForSection(int sectionIndex)
    {
        foreach (Transform t in starContainer)
            Destroy(t.gameObject);

        var section = sections[sectionIndex];

        // Fila 1 (ej: 3 notas)
        foreach (var timed in section.row1)
        {
            Instantiate(starPrefab, starContainer)
                .GetComponent<NoteStar>()
                .Setup(timed.note);
        }

        // Fila 2 (ej: 5 notas)
        foreach (var timed in section.row2)
        {
            Instantiate(starPrefab, starContainer)
                .GetComponent<NoteStar>()
                .Setup(timed.note);
        }
    }


    // ---------------------------------------------------------
    // CONTROL DE SECCIÓN
    // ---------------------------------------------------------
    IEnumerator StartSection()
    {
        GenerateStarsForSection(currentSection);

        yield return new WaitForSeconds(0.6f);
        TTSManager.Instance.Speak("Escucha con atención.");
        yield return new WaitForSeconds(1.2f);

        yield return StartCoroutine(PlayReferenceAudio());

        yield return new WaitForSeconds(0.5f);
        TTSManager.Instance.Speak("Ahora tú.");

        yield return new WaitForSeconds(1f);

        currentRepeat = 0;
        StartPlayerRepeat();
    }

    // ---------------------------------------------------------
    // AUDIO + ESTRELLAS SINCRONIZADAS
    // ---------------------------------------------------------
    IEnumerator PlayReferenceAudio()
    {
        phase = Level1Phase.AutoPlay;
        DisableAllKeys();

        var section = sections[currentSection];

        audioSource.clip = section.referenceAudio;
        audioSource.Play();

        float startTime = Time.time;

        int starIndex = 0;

        // Fila 1
        foreach (var timed in section.row1)
        {
            float wait = startTime + timed.time - Time.time;
            if (wait > 0)
                yield return new WaitForSeconds(wait);

            starContainer.GetChild(starIndex++)
                .GetComponent<NoteStar>()
                .ShowColor();
        }

        // Fila 2
        foreach (var timed in section.row2)
        {
            float wait = startTime + timed.time - Time.time;
            if (wait > 0)
                yield return new WaitForSeconds(wait);

            starContainer.GetChild(starIndex++)
                .GetComponent<NoteStar>()
                .ShowColor();
        }

        yield return new WaitUntil(() => !audioSource.isPlaying);
        ClearStars();
    }


    // ---------------------------------------------------------
    // REPETICIÓN DEL NIÑO (SOLO FILA 1)
    // ---------------------------------------------------------
    void StartPlayerRepeat()
    {
        phase = Level1Phase.PlayerRepeat;
        waitingForInput = true;
        playerNoteIndex = 0;

        HighlightStar(playerNoteIndex);
        EnableOnlyCorrectKey();
    }

    void OnKeyPressed(NoteData pressed)
    {
        if (!waitingForInput || phase != Level1Phase.PlayerRepeat)
            return;

        var expected = GetRow1Notes()[playerNoteIndex];

        if (pressed.noteName == expected.note.noteName)
        {
            FeedbackManager.Instance.RegisterHit();
            PaintStar(playerNoteIndex);
            playerNoteIndex++;

            if (playerNoteIndex >= GetRow1Notes().Length)
            {
                waitingForInput = false;
                StartCoroutine(HandleRepeatEnd());
            }
            else
            {
                HighlightStar(playerNoteIndex);
                EnableOnlyCorrectKey();
            }
        }
        else
        {
            FeedbackManager.Instance.RegisterMistake();
        }
    }

    // ---------------------------------------------------------
    // REPETICIONES
    // ---------------------------------------------------------
    IEnumerator HandleRepeatEnd()
    {
        currentRepeat++;
        yield return new WaitForSeconds(0.8f);

        if (currentRepeat < 2)
        {
            StartPlayerRepeat();
        }
        else
        {
            currentSection++;

            if (currentSection < sections.Length)
                StartCoroutine(StartSection());
            else
                ActivityConnector.Instance.OnWin();
        }
    }

    // ---------------------------------------------------------
    // UTILIDADES
    // ---------------------------------------------------------
    TimedNote[] GetRow1Notes()
    {
        return sections[currentSection].row1;
    }


    void HighlightStar(int index)
    {
        starContainer.GetChild(index)
            .GetComponent<NoteStar>()
            .ShowColor();
    }

    void PaintStar(int index)
    {
        starContainer.GetChild(index)
            .GetComponent<NoteStar>()
            .ShowColor();
    }

    void ClearStars()
    {
        foreach (Transform t in starContainer)
            t.GetComponent<NoteStar>().ResetToInitial();
    }

    // ---------------------------------------------------------
    // TECLAS
    // ---------------------------------------------------------
    void EnableOnlyCorrectKey()
    {
        var expected = GetRow1Notes()[playerNoteIndex];

        foreach (var key in pianoKeys)
        {
            bool ok = key.noteData.noteName == expected.note.noteName;
            key.SetKeyEnabled(ok);

            if (ok) key.ShowHelp();
            else key.ResetVisualHelp();
        }
    }

    void DisableAllKeys()
    {
        foreach (var key in pianoKeys)
        {
            key.SetKeyEnabled(false);
            key.ResetVisualHelp();
        }
    }

    void LinkKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }
}
