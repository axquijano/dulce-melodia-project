using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Para registrar los acierto, el tiempo mientras el niño esta jugando
public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    

    [Header("UI")]
    public ProgressBar progressBar;
    public TMP_Text timeText;

    private int hits; 
    private int mistakes;
    private float maxMistakes;
    public float GetTime() => idleTimer;


    private float idleTimer = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SetMaxMistakes (float value) {
        maxMistakes = value;

        if(progressBar == null) return ;
        progressBar.SetMaxMistakes(value);
    }

    void Update()
    {
        idleTimer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(idleTimer / 60f);
        int seconds = Mathf.FloorToInt(idleTimer % 60f);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    public void RegisterHit()
    {
        hits++;
        UpdateUI();
    } 

    public void RegisterMistake()
    {
        mistakes++;
        UpdateUI();
    }

    void UpdateUI()
    {
        progressBar.UpdateProgressBar(mistakes); 
    }

    public int getMistakes() {
        return mistakes;
    }


}
