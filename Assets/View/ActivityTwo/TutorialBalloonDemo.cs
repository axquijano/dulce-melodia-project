using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class TutorialBalloonDemo : MonoBehaviour
{
    [Header("Expected Note")]
    public NoteData expectedNote;

    [Header("UI References")]
    public Image balloonImage;
    public TMP_Text letter;

    [Header("Optional")]
    public VisualFeedback visualFeedback;

    [Header("Movement")]
    public float tutorialSpeed = 80f;
    public RectTransform parentPanel;

    private bool exploded = false;
    private bool moving = false;
    private Coroutine pulseRoutine;

    public Action OnMissed;

    void Start()
    {
        SetupVisual();
        StartPulse();
    }

    void Update()
    {
        if (!moving || exploded) return;

        transform.localPosition += Vector3.up * tutorialSpeed * Time.deltaTime;

        if (ReachedTop())
        {
            SimulateMiss();
            moving = false;
            OnMissed?.Invoke();
        }
    }

    bool ReachedTop()
    {
        if (parentPanel == null) return false;

        float halfHeight = ((RectTransform)transform).rect.height * 0.5f;

        Vector3[] corners = new Vector3[4];
        parentPanel.GetWorldCorners(corners);

        float worldLimitY = corners[1].y;

        Vector3 balloonTop =
            ((RectTransform)transform).TransformPoint(new Vector3(0, halfHeight, 0));

        return balloonTop.y > worldLimitY;
    }


    public void StartMoving()
    {
        exploded = false;
        moving = true;
    }

    // -------------------------
    // VISUAL SETUP
    // -------------------------

    void SetupVisual()
    {
        if (expectedNote == null) return;

        if (balloonImage != null)
            balloonImage.color = expectedNote.color;

        if (letter != null)
        {
            letter.text = expectedNote.noteName;
            letter.gameObject.SetActive(true);
        }
    }

    // -------------------------
    // TRY EXPLODE
    // -------------------------

    public bool TryExplode(NoteData pressedNote)
    {
        if (exploded) return false;
        if (pressedNote != expectedNote) return false;

        exploded = true;
        StopPulse();

        StartCoroutine(ExplodeSequence());
        return true;
    }

    IEnumerator ExplodeSequence()
    {
        TTSManager.Instance.Speak("¡Muy bien!");

        if (visualFeedback != null)
            visualFeedback.ShowNextReward();

        transform.localScale = Vector3.one * 1.2f;
        yield return new WaitForSeconds(0.2f);
        transform.localScale = Vector3.one;

        if (balloonImage != null)
            balloonImage.gameObject.SetActive(false);

        if (letter != null)
            letter.gameObject.SetActive(false);
    }

    // -------------------------
    // MISS
    // -------------------------

    public void SimulateMiss()
    {
        if (exploded) return;

        StopPulse();

        if (balloonImage != null)
            balloonImage.color = Color.gray;

        if (letter != null)
            letter.gameObject.SetActive(false);

        exploded = true;
    }

    // -------------------------
    // PULSE
    // -------------------------

    void StartPulse()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        pulseRoutine = StartCoroutine(Pulse());
    }

    void StopPulse()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);
    }

    IEnumerator Pulse()
    {
        float speed = 1.2f;
        float minScale = 0.95f;
        float maxScale = 1.05f;

        while (!exploded)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f);
            float scale = Mathf.Lerp(minScale, maxScale, t);
            transform.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    // -------------------------
    // RESET
    // -------------------------

    public void ResetBalloon(NoteData newNote)
    {
        expectedNote = newNote;
        exploded = false;
        moving = false;

        transform.localScale = Vector3.one;

        if (balloonImage != null)
            balloonImage.gameObject.SetActive(true);

        if (letter != null)
            letter.gameObject.SetActive(true);

        SetupVisual();
        StartPulse();
    }
}
