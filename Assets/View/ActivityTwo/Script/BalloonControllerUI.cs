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

    private bool isDead = false;


    void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (balloonImage != null)
            originalColor = balloonImage.color;
    }

    /* void Update()
    {
        if (isDead) return; 
        float limitY = parentPanel.rect.height * 0.5f;
        rect.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
        float halfHeight  = rect.rect.height * 0.5f ;

        if (rect.anchoredPosition.y - halfHeight  > limitY)
        {
            isDead = true;
            manager.RegisterBalloonMiss(this);
            Destroy(gameObject);
        }
    } */

    void Update()
    {
        if (isDead) return;

        rect.anchoredPosition += new Vector2(0, speed * Time.deltaTime);

        float halfHeight = rect.rect.height * 0.5f;

        // Límite superior en coordenadas del mundo
        Vector3[] corners = new Vector3[4];
        parentPanel.GetWorldCorners(corners);
        float worldLimitY = corners[1].y;

        // Parte superior del globo
        Vector3 balloonTop = rect.TransformPoint(new Vector3(0, halfHeight, 0));

        if (balloonTop.y > worldLimitY)
        {
            isDead = true;
            manager.RegisterBalloonMiss(this);
            Destroy(gameObject);
        }
    }


   /*  public void Pop()
    {
        ActivityTwoManager.Instance.RegisterBalloonHit(noteData);
        Destroy(gameObject);
    } */

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

}
