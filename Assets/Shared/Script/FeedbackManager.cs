using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Para registrar los acierto, el tiempo mientras el niño esta jugando
public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    [Header("UI")]
    public TMP_Text hitsText;
    public TMP_Text mistakesText;
    public TMP_Text timeText;

   /*  [Header("Config")]
    public int mistakesBeforeHelp = 3;   // activar ayuda después de X errores
    public float idleTimeBeforeHelp = 5f; // activar ayuda si pasa X segundos sin tocar */

    private int hits;
    private int mistakes;

    public int GetHits() => hits;
    public float GetTime() => idleTimer;


    private float idleTimer = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Update()
    {
        idleTimer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(idleTimer / 60f);
        int seconds = Mathf.FloorToInt(idleTimer % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
        /* 
        if (idleTimer >= idleTimeBeforeHelp)
        {
            SequencePlayer.Instance.ShowHelpCurrentKey();
            idleTimer = 0f; // reiniciar el contador 
        }*/
    }

    public void RegisterHit()
    {
        hits++;
      /*   idleTimer = 0f; */
        UpdateUI();
    }

    public void RegisterMistake()
    {
        mistakes++;
      /*   idleTimer = 0f; */
        UpdateUI();

     /*    if (mistakes % mistakesBeforeHelp == 0)
        {
            SequencePlayer.Instance.ShowHelpCurrentKey();
        } */
    }

    void UpdateUI()
    {
        hitsText.text = hits.ToString();
        mistakesText.text = mistakes.ToString();
    }

    public int getMistakes() {
        return mistakes;
    }


}
