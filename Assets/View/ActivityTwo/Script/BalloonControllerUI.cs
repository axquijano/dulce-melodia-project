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

    public string id = System.Guid.NewGuid().ToString();

    public ActivityTwoManager manager;

    public RectTransform parentPanel;

    private bool wasMissRegistered = false;
    private bool isDead = false;/*  */


    void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (balloonImage != null)
            originalColor = balloonImage.color;
    }

    void Update()
    {
        rect.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        float halfHeight = rect.rect.height * 0.5f;

        Vector3[] corners = new Vector3[4];
        parentPanel.GetWorldCorners(corners);
        float worldLimitY = corners[1].y;

        Vector3 balloonTop = rect.TransformPoint(new Vector3(0, halfHeight, 0));

        // Globo perdido (solo una vez)
        if (!wasMissRegistered && balloonTop.y > worldLimitY)
        {
            wasMissRegistered = true;
            isDead = true; // ya no es tocable

            SetMissedVisual();        
            manager.RegisterBalloonMiss(this);
        }

        if (balloonTop.y > worldLimitY + 200f)
        {
            Destroy(gameObject);
        }
    }

    public void Pop()
    {
       if (isDead) return;
        isDead = true;
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
        else if (displayColor){
            balloonImage.color = data.color;
        }
        else{
            balloonImage.color = originalColor;
            letter.color = data.color;
        }
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

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void SetMissedVisual()
    {
        if (balloonImage != null)
            balloonImage.color = Color.gray;

        if (letter != null)
            letter.gameObject.SetActive(false);
    }

}
