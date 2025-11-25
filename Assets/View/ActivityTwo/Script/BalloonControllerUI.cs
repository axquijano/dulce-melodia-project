using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BalloonControllerUI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 120f;
    private RectTransform rect;

    [Header("References")]
    [SerializeField] private Image balloonImage;
    [SerializeField] private TextMeshProUGUI letter;
    [SerializeField] private VisualFeedback visualFeedback;

    [Header("Settings")]
    [SerializeField] private Color blackColor = Color.black;

    private Color originalColor;
    public NoteData noteData;

    public Sprite[] imagenes;
    public ActivityTwoManager manager;
    public RectTransform parentPanel;

    private bool wasMissRegistered = false;
    private bool isDead = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (balloonImage != null)
            originalColor = balloonImage.color;
    }

    void Update()
    {
        if (isDead) return;

        rect.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        float halfHeight = rect.rect.height * 0.5f;

        Vector3[] corners = new Vector3[4];
        parentPanel.GetWorldCorners(corners);
        float worldLimitY = corners[1].y;

        Vector3 balloonTop = rect.TransformPoint(new Vector3(0, halfHeight, 0));

        // Globo perdido (una sola vez)
        if (!wasMissRegistered && balloonTop.y > worldLimitY)
        {
            wasMissRegistered = true;
            isDead = true;

            SetMissedVisual();
            manager.RegisterBalloonMiss(this);
        }

        if (balloonTop.y > worldLimitY + 200f)
        {
            Destroy(gameObject);
        }
    }

    // -------------------------
    // SETUP
    // -------------------------
    public void Setup(NoteData data, bool displayColor, bool displayLetter, bool isBlack)
    {
        noteData = data;

        // Imagen aleatoria
        if (imagenes != null && imagenes.Length > 0)
            balloonImage.sprite = imagenes[Random.Range(0, imagenes.Length)];

        // Color
        if (isBlack)
        {
            balloonImage.color = blackColor;
        }
        else if (displayColor)
        {
            balloonImage.color = data.color;
        }
        else
        {
            balloonImage.color = originalColor;
            letter.color = data.color;
        }

        // Letra
        if (displayLetter)
        {
            letter.gameObject.SetActive(true);
            letter.text = data.noteName;
        }
        else
        {
            letter.gameObject.SetActive(false);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // -------------------------
    // HIT FEEDBACK (CLAVE)
    // -------------------------
    public void PlayHitFeedbackAndPop()
    {
        if (isDead) return;
        isDead = true;

        // Ocultar globo y letra
        if (balloonImage != null)
            balloonImage.enabled = false;

        if (letter != null)
            letter.gameObject.SetActive(false);

        // Estímulo visual
        if (visualFeedback != null)
            visualFeedback.ShowNextReward();

        StartCoroutine(DestroyAfterFeedback());
    }

    IEnumerator DestroyAfterFeedback()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }

    // -------------------------
    // MISS
    // -------------------------
    void SetMissedVisual()
    {
        if (balloonImage != null)
            balloonImage.color = Color.gray;

        if (letter != null)
            letter.gameObject.SetActive(false);
    }
}
