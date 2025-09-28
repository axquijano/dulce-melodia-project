using UnityEngine;

public class ActivityConnector : MonoBehaviour
{
    public static ActivityConnector Instance;

    private int hits = 0;
    private int mistakes = 0;
    private float timer = 0f;
    private bool playing = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (playing)
            timer += Time.deltaTime;
    }

    public void StartLevel()
    {
        hits = 0;
        mistakes = 0;
        timer = 0;
        playing = true;
    }

    public void RegisterHit()
    {
        hits++;
    }

    public void RegisterMistake()
    {
        mistakes++;
    }

    public void OnLose()
    {
        playing = false;

        var data = ProfilesManager.Instance.GetCurrentLevelData();
        data.retries++;

        ProfilesManager.Instance.UpdateCurrentLevelData(data);
    }

    public void OnWin()
    {
        playing = false;

        ChildProfile p = ProfilesManager.Instance.currentProfile;
        LevelProgressData d = ProfilesManager.Instance.GetCurrentLevelData();

        // best time
        if (d.bestTime < 0 || timer < d.bestTime)
            d.bestTime = timer;

        // hits
        if (hits > d.bestHits)
            d.bestHits = hits;

        // mistakes
        if (d.bestMistakes < 0 || mistakes < d.bestMistakes)
            d.bestMistakes = mistakes;

        // retries suman 1 si ganó
        d.retries++;

        // estrellas
        if (mistakes == 0) d.stars = 3;
        else if (mistakes <= 2) d.stars = 2;
        else d.stars = 1;

        ProfilesManager.Instance.UpdateCurrentLevelData(d);

        // avanzar nivel/actividad
        ProfilesManager.Instance.AdvanceLevelOrActivity();
    }
}
