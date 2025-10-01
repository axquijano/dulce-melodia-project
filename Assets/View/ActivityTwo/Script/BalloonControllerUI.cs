using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BalloonControllerUI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 120f;

    private RectTransform rect;

    [Header("References")]
    [SerializeField] private Image balloonImage;
    [SerializeField] private TextMeshProUGUI letter;

    [Header("Settings")]
    [SerializeField] private Color blackColor = Color.black;

    private Color originalColor;
    public NoteData noteData;  // la nota asociada a este globo

    public Sprite[] imagenes ;
    void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (balloonImage != null)
            originalColor = balloonImage.color;
    }

    void Update()
    {
        rect.anchoredPosition += new Vector2(0, speed * Time.deltaTime);

        if (rect.anchoredPosition.y > Screen.height + 200f)
        {
            ActivityTwoManager.Instance.RegisterBalloonMiss(noteData);
            Destroy(gameObject);
        }
    }

    public void Pop()
    {
        ActivityTwoManager.Instance.RegisterBalloonHit(noteData);
        Destroy(gameObject);
    }

    public void Setup(NoteData data, bool displayColor, bool displayLetter, bool isBlack)
    {
        noteData = data;

        // --- Imagen aleatoria ---
        if (imagenes != null && imagenes.Length > 0)
        {
            balloonImage.sprite = imagenes[Random.Range(0, imagenes.Length)];
        }
        // --- COLOR ---
        if (isBlack)
            balloonImage.color = blackColor;
        else if (displayColor)
            balloonImage.color = data.color;
        else
            balloonImage.color = originalColor;

        // --- LETRA ---
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
}
