using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialBalloonDemo : MonoBehaviour
{
    [Header("Expected note")]
    public NoteData expectedNote;

    [Header("UI")]
    public Image balloonImage;
    public TMP_Text letter;

    [Header("Feedback")]
    public VisualFeedback feedback;

    private bool exploded = false;

    void Start()
    {
        // Mostrar visual del globo
        balloonImage.color = expectedNote.color;
        letter.text = expectedNote.noteName;
        letter.gameObject.SetActive(true);
    }

    public bool TryExplode(NoteData pressedNote)
    {
        if (exploded) return false;
        if (pressedNote != expectedNote) return false;

        exploded = true;

        balloonImage.gameObject.SetActive(false);
        letter.gameObject.SetActive(false);

        if (feedback != null)
            feedback.ShowNextReward();

        TTSManager.Instance.Speak("¡Muy bien!");

        return true;
    }
}
