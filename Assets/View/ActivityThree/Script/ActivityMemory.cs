using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ActivityMemory : MonoBehaviour
{
    [Header("UI Stars")]
    public Transform row1Container;
    public Transform row2Container;
    public GameObject starPrefab;

    [Header("Piano")]
    public PianoKey[] pianoKeys;

    [Header("Song")]
    public SongData songData;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Avatar")]
    public StudentAvatarDatabase avatarDatabase;
    public Image avatarImage;
    public TMP_Text messageText;

    [Header("Rules")]
    public int maxMistakes = 12; // 🔵 Barra máximo 12

    int currentSection = 0;
    int playerNoteIndex = 0;
    int totalNotes = 0;
    int mistakeCount = 0;

    private int helpMistakeCount = 0; // 🟣 SOLO para activar ayuda

    bool waitingForInput = false;

    enum LevelPhase { None, AutoPlay, PlayerRepeat }
    LevelPhase phase = LevelPhase.None;

    StudentAvatarData currentAvatar;
    string childName;

    bool isMemoryLevel => GameFlowManager.Instance.selectedLevel == 1;

    // ---------------------------------------------------------
    void Start()
    {
        FeedbackManager.Instance.EnableManualTimerMode();
        FeedbackManager.Instance.ResetTimer();
        FeedbackManager.Instance.ResetStats();
        FeedbackManager.Instance.SetMaxMistakes(maxMistakes);

        SetupAvatar();
        ActivityConnector.Instance.StartLevel();
        LinkKeys();

        // 🔊 Desactivar sonido interno de teclas
        foreach (var key in pianoKeys)
            key.allowInternalSound = false;

        StartCoroutine(IntroSequence());
    }

    void SetupAvatar()
    {
        var profile = ProfilesManager.Instance.currentProfile;
        childName = profile.childName;

        currentAvatar = avatarDatabase.GetById(profile.avatarId);

        if (avatarImage != null)
            avatarImage.sprite = currentAvatar.avatarSprite;
    }

    // ---------------------------------------------------------
    IEnumerator IntroSequence()
    {
        GenerateStarsForSection(currentSection);

        yield return Speak(
            $"Hola {childName}, vamos a escuchar la música juntos.",
            currentAvatar.happySprite,
            3f
        );

        StartCoroutine(StartSection());
    }

    // ---------------------------------------------------------
    void GenerateStarsForSection(int sectionIndex)
    {
        foreach (Transform t in row1Container) Destroy(t.gameObject);
        foreach (Transform t in row2Container) Destroy(t.gameObject);

        var section = songData.sections[sectionIndex];

        foreach (var timed in section.row1)
            Instantiate(starPrefab, row1Container)
                .GetComponent<MemoryNoteView>()
                .Setup(timed.note, timed.isGhost);

        foreach (var timed in section.row2)
            Instantiate(starPrefab, row2Container)
                .GetComponent<MemoryNoteView>()
                .Setup(timed.note, timed.isGhost);
    }

    // ---------------------------------------------------------
    IEnumerator StartSection()
    {
        if (!isMemoryLevel)
        {
            yield return Speak("Escucha con atención.",
                currentAvatar.avatarSprite, 2f);

            yield return StartCoroutine(PlayReferenceAudio());

            ResetAllStars();

            yield return Speak("Ahora es tu turno.",
                currentAvatar.happySprite, 2f);

            StartPlayerRepeat();
        }
        else
        {
            yield return Speak("Escucha con mucha atención.",
                currentAvatar.avatarSprite, 2.5f);

            yield return StartCoroutine(PlayReferenceAudio());

            yield return Speak("Vamos a escuchar una vez más.",
                currentAvatar.avatarSprite, 2.5f);

            yield return StartCoroutine(PlayReferenceAudio());

            yield return Speak(
                "Ahora algunas notas desaparecerán. Trata de recordarlas.",
                currentAvatar.avatarSprite, 3f);

            ResetAllStars();
            ActivateGhosts();

            StartPlayerRepeat();
        }
    }

    // ---------------------------------------------------------
    IEnumerator PlayReferenceAudio()
    {
        phase = LevelPhase.AutoPlay;
        waitingForInput = false;

        FeedbackManager.Instance.StopTimer();
        DisableAllKeys();
        ResetAllStars();

        var section = songData.sections[currentSection];
        audioSource.clip = section.referenceAudio;
        audioSource.Play();

        float startTime = Time.time;

        for (int i = 0; i < section.row1.Length; i++)
        {
            float wait = startTime + section.row1[i].time - Time.time;
            if (wait > 0) yield return new WaitForSeconds(wait);

            row1Container.GetChild(i)
                .GetComponent<MemoryNoteView>()
                .ShowListening();
        }

        for (int i = 0; i < section.row2.Length; i++)
        {
            float wait = startTime + section.row2[i].time - Time.time;
            if (wait > 0) yield return new WaitForSeconds(wait);

            row2Container.GetChild(i)
                .GetComponent<MemoryNoteView>()
                .ShowListening();
        }

        yield return new WaitUntil(() => !audioSource.isPlaying);

        ResetAllStars();
    }

    // ---------------------------------------------------------
    void StartPlayerRepeat()
    {
        phase = LevelPhase.PlayerRepeat;
        waitingForInput = true;
        mistakeCount = 0;
        helpMistakeCount = 0;
        playerNoteIndex = 0;

        totalNotes =
            songData.sections[currentSection].row1.Length +
            songData.sections[currentSection].row2.Length;

        HighlightStar(playerNoteIndex);
        EnableAllKeys();

        FeedbackManager.Instance.StartTimer();
    }

    void OnKeyPressed(NoteData pressed)
    {
        if (!waitingForInput) return;

        var expected = GetExpectedNote(playerNoteIndex);

        if (pressed == expected.note)
        {
            FeedbackManager.Instance.RegisterHit();

            // 🔊 Reproducir sonido correcto
            PlayTimedNoteSound(expected);

            PaintStar(playerNoteIndex);

            playerNoteIndex++;
            helpMistakeCount = 0;

            if (playerNoteIndex >= totalNotes)
            {
                waitingForInput = false;
                FeedbackManager.Instance.StopTimer();
                StartCoroutine(HandleSectionEnd());
            }
            else
            {
                HighlightStar(playerNoteIndex);
            }
        }
        else
        {
            mistakeCount++;
            helpMistakeCount++;

            FeedbackManager.Instance.RegisterMistake();
            GetStarByIndex(playerNoteIndex).ShowError();

            if (isMemoryLevel && helpMistakeCount >= 2)
            {
                waitingForInput = false;
                FeedbackManager.Instance.StopTimer();
                StartCoroutine(RestartWithReference());
            }
        }
    }

    // ---------------------------------------------------------
    void PlayTimedNoteSound(TimedNote timedNote)
    {
        AudioClip clip =
            timedNote.overrideSound != null
            ? timedNote.overrideSound
            : timedNote.note.sound;

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    // ---------------------------------------------------------
    IEnumerator RestartWithReference()
    {
        helpMistakeCount = 0;

        yield return Speak(
            "Escuchemos la secuencia otra vez.",
            currentAvatar.avatarSprite,
            3f
        );

        yield return StartCoroutine(PlayReferenceAudio());

        ResetAllStars();
        ActivateGhosts();

        StartPlayerRepeat();
    }

    // ---------------------------------------------------------
    IEnumerator HandleSectionEnd()
    {
        yield return new WaitForSeconds(1f);

        currentSection++;

        if (currentSection < songData.sections.Count)
        {
            yield return Speak(
                "Excelente, vamos a la siguiente parte.",
                currentAvatar.celebrationSprite,
                3f
            );

            GenerateStarsForSection(currentSection);
            StartCoroutine(StartSection());
        }
        else
        {
            yield return Speak(
                $"Muy bien {childName}, completaste este nivel.",
                currentAvatar.celebrationSprite,
                3f
            );

            Debug.Log("Tiempo total: " + FeedbackManager.Instance.GetTime());
            Debug.Log("Errores: " + FeedbackManager.Instance.GetMistakes());
            Debug.Log("Aciertos: " + FeedbackManager.Instance.GetHits());

            ActivityConnector.Instance.OnWin();
        }
    }

    // ---------------------------------------------------------
    void ActivateGhosts()
    {
        foreach (Transform t in row1Container)
            t.GetComponent<MemoryNoteView>().ActivateGhost();

        foreach (Transform t in row2Container)
            t.GetComponent<MemoryNoteView>().ActivateGhost();
    }

    void HighlightStar(int index)
    {
        GetStarByIndex(index).ShowTurn();
    }

    void PaintStar(int index)
    {
        GetStarByIndex(index).ShowCorrect();
    }

    void ResetAllStars()
    {
        foreach (Transform t in row1Container)
            t.GetComponent<MemoryNoteView>().ResetVisual();

        foreach (Transform t in row2Container)
            t.GetComponent<MemoryNoteView>().ResetVisual();
    }

    MemoryNoteView GetStarByIndex(int index)
    {
        if (index < row1Container.childCount)
            return row1Container.GetChild(index).GetComponent<MemoryNoteView>();
        else
            return row2Container
                .GetChild(index - row1Container.childCount)
                .GetComponent<MemoryNoteView>();
    }

    void EnableAllKeys()
    {
        foreach (var key in pianoKeys)
            key.SetKeyEnabled(true);
    }

    void DisableAllKeys()
    {
        foreach (var key in pianoKeys)
            key.SetKeyEnabled(false);
    }

    void LinkKeys()
    {
        foreach (var key in pianoKeys)
            key.onKeyPressed += OnKeyPressed;
    }

    IEnumerator Speak(string text, Sprite sprite, float wait)
    {
        DisableAllKeys();
        FeedbackManager.Instance.StopTimer();

        if (avatarImage != null && sprite != null)
            avatarImage.sprite = sprite;

        messageText.text = text;
        TTSManager.Instance.Speak(text);

        yield return new WaitForSeconds(wait);

        messageText.text = "";
    }

    TimedNote GetExpectedNote(int index)
    {
        var section = songData.sections[currentSection];

        if (index < section.row1.Length)
            return section.row1[index];
        else
            return section.row2[index - section.row1.Length];
    }
}
