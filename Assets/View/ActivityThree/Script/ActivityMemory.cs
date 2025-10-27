using UnityEngine;
using System.Collections;

public class ActivityMemory : MonoBehaviour
{
    // ---------------------------------------------------------
    // UI
    // ---------------------------------------------------------
    [Header("UI")]
    public Transform row1Container;
    public Transform row2Container;
    public GameObject starPrefab;

    // ---------------------------------------------------------
    // INPUT
    // ---------------------------------------------------------
    [Header("Input")]
    public PianoKey[] pianoKeys;

    // ---------------------------------------------------------
    // SONG DATA
    // ---------------------------------------------------------
    [Header("Song")]
    public SongData songData;

    // ---------------------------------------------------------
    // AUDIO
    // ---------------------------------------------------------
    [Header("Audio")]
    public AudioSource audioSource;

    // ---------------------------------------------------------
    // RULES
    // ---------------------------------------------------------
    [Header("Rules")]
    public int maxMistakesPerRepeat = 3;

    // ---------------------------------------------------------
    // STATE
    // ---------------------------------------------------------
    int currentSection = 0;
    int currentRepeat = 0;
    int playerNoteIndex = 0;
    int totalNotes = 0;
    int mistakesThisRepeat = 0;

    bool waitingForInput = false;

    enum LevelPhase { None, AutoPlay, PlayerRepeat }
    LevelPhase phase = LevelPhase.None;

    // ---------------------------------------------------------
    // LEVEL FLAGS
    // ---------------------------------------------------------
    bool isIntroLevel => GameFlowManager.Instance.selectedLevel == 0; // Nivel 1
    bool isMemoryLevel => GameFlowManager.Instance.selectedLevel == 1; // Nivel 2

    bool isFirstAttempt => currentRepeat == 0;
    bool isSecondAttempt => currentRepeat == 1;

    // ---------------------------------------------------------
    // START
    // ---------------------------------------------------------
    void Start()
    {
        ActivityConnector.Instance.StartLevel();
        LinkKeys();
        StartCoroutine(StartSection());
    }

    // ---------------------------------------------------------
    // GENERAR ESTRELLAS
    // ---------------------------------------------------------
    void GenerateStarsForSection(int sectionIndex)
    {
        foreach (Transform t in row1Container) Destroy(t.gameObject);
        foreach (Transform t in row2Container) Destroy(t.gameObject);

        var section = songData.sections[sectionIndex];

        foreach (var timed in section.row1)
        {
            Instantiate(starPrefab, row1Container)
                .GetComponent<NoteStar>()
                .Setup(timed.note, timed.isGhost);
        }

        foreach (var timed in section.row2)
        {
            Instantiate(starPrefab, row2Container)
                .GetComponent<NoteStar>()
                .Setup(timed.note, timed.isGhost);
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

        yield return new WaitForSeconds(0.8f);

        currentRepeat = 0;
        StartPlayerRepeat();
    }

    // ---------------------------------------------------------
    // AUTOPLAY (GUÍA)
    // ---------------------------------------------------------
    IEnumerator PlayReferenceAudio()
    {
        phase = LevelPhase.AutoPlay;
        waitingForInput = false;
        DisableAllKeys();

        var section = songData.sections[currentSection];
        audioSource.clip = section.referenceAudio;
        audioSource.Play();

        float startTime = Time.time;

        for (int i = 0; i < section.row1.Length; i++)
        {
            var timed = section.row1[i];
            float wait = startTime + timed.time - Time.time;
            if (wait > 0) yield return new WaitForSeconds(wait);

            row1Container.GetChild(i).GetComponent<NoteStar>().ShowColor();
        }

        for (int i = 0; i < section.row2.Length; i++)
        {
            var timed = section.row2[i];
            float wait = startTime + timed.time - Time.time;
            if (wait > 0) yield return new WaitForSeconds(wait);

            row2Container.GetChild(i).GetComponent<NoteStar>().ShowColor();
        }

        yield return new WaitUntil(() => !audioSource.isPlaying);
        ClearStars();
    }

    // ---------------------------------------------------------
    // TURNO DEL NIÑO
    // ---------------------------------------------------------
    void StartPlayerRepeat()
    {
        phase = LevelPhase.PlayerRepeat;
        waitingForInput = true;
        playerNoteIndex = 0;
        mistakesThisRepeat = 0;

        totalNotes =
            songData.sections[currentSection].row1.Length +
            songData.sections[currentSection].row2.Length;

        if (isMemoryLevel && isSecondAttempt)
            ShowGhosts();

        HighlightStar(playerNoteIndex);
        EnableOnlyCorrectKey();
    }

    void OnKeyPressed(NoteData pressed)
    {
        if (!waitingForInput || phase != LevelPhase.PlayerRepeat)
            return;

        var expected = GetExpectedNote(playerNoteIndex);

        if (pressed.noteName == expected.note.noteName)
        {
            FeedbackManager.Instance.RegisterHit();
            ActivityConnector.Instance.RegisterHit();

            PaintStar(playerNoteIndex);
            playerNoteIndex++;

            if (playerNoteIndex >= totalNotes)
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

            if (isMemoryLevel)
            {
                ActivityConnector.Instance.RegisterMistake();
                mistakesThisRepeat++;

                if (mistakesThisRepeat >= maxMistakesPerRepeat)
                {
                    waitingForInput = false;
                    StartCoroutine(HandleRepeatFail());
                }
            }
        }
    }

    // ---------------------------------------------------------
    // REPETICIONES
    // ---------------------------------------------------------
    IEnumerator HandleRepeatEnd()
    {
        currentRepeat++;
        yield return new WaitForSeconds(0.8f);

        if (isMemoryLevel && currentRepeat == 1)
        {
            ClearStars();
            ShowGhosts();
            yield return new WaitForSeconds(0.6f);
        }

        if (currentRepeat < 2)
        {
            if (!(isMemoryLevel && currentRepeat == 1))
                ClearStars();

            StartPlayerRepeat();
        }
        else
        {
            currentSection++;

            if (currentSection < songData.sections.Count)
                StartCoroutine(StartSection());
            else
                ActivityConnector.Instance.OnWin();
        }
    }

    IEnumerator HandleRepeatFail()
    {
        DisableAllKeys();
        yield return new WaitForSeconds(0.8f);

        currentRepeat++;

        if (currentRepeat < 2)
        {
            ClearStars();
            StartPlayerRepeat();
        }
        else
        {
            ActivityConnector.Instance.OnLose();
        }
    }

    // ---------------------------------------------------------
    // UTILIDADES
    // ---------------------------------------------------------
    TimedNote GetExpectedNote(int index)
    {
        var section = songData.sections[currentSection];

        if (index < section.row1.Length)
            return section.row1[index];
        else
            return section.row2[index - section.row1.Length];
    }

    // ---------------------------------------------------------
    // VISUALES
    // ---------------------------------------------------------
    void HighlightStar(int index)
    {
        GetStarByIndex(index).ShowColor();
    }

    void PaintStar(int index)
    {
        var star = GetStarByIndex(index);
        var expected = GetExpectedNote(index);

        star.RevealGhost(expected.note.noteName);
        star.ShowColor();
    }

    NoteStar GetStarByIndex(int index)
    {
        if (index < row1Container.childCount)
            return row1Container.GetChild(index).GetComponent<NoteStar>();
        else
            return row2Container
                .GetChild(index - row1Container.childCount)
                .GetComponent<NoteStar>();
    }

    void ClearStars()
    {
        foreach (Transform t in row1Container)
            t.GetComponent<NoteStar>().ResetToInitial();

        foreach (Transform t in row2Container)
            t.GetComponent<NoteStar>().ResetToInitial();
    }

    void ShowGhosts()
    {
        foreach (Transform t in row1Container)
            t.GetComponent<NoteStar>().SetGhostVisible(true);

        foreach (Transform t in row2Container)
            t.GetComponent<NoteStar>().SetGhostVisible(true);
    }

    // ---------------------------------------------------------
    // TECLADO
    // ---------------------------------------------------------
    void EnableOnlyCorrectKey()
    {
        var expected = GetExpectedNote(playerNoteIndex);

        bool allowKeyboardHelp = isIntroLevel && isFirstAttempt;

        foreach (var key in pianoKeys)
        {
            bool ok = key.noteData.noteName == expected.note.noteName;
            key.SetKeyEnabled(ok);

            if (allowKeyboardHelp && ok)
                key.ShowHelp();
            else
                key.ResetVisualHelp();
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

    // ---------------------------------------------------------
    // BOTÓN REPETIR
    // ---------------------------------------------------------
    public void OnRepeatButtonPressed()
    {
        if (phase == LevelPhase.AutoPlay)
            return;

        StopAllCoroutines();
        audioSource.Stop();

        waitingForInput = false;
        DisableAllKeys();
        ClearStars();

        StartCoroutine(RepeatReference());
    }

    IEnumerator RepeatReference()
    {
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(PlayReferenceAudio());

        yield return new WaitForSeconds(0.5f);
        TTSManager.Instance.Speak("Ahora tú.");

        yield return new WaitForSeconds(0.8f);
        StartPlayerRepeat();
    }
}
