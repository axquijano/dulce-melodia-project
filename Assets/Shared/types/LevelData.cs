[System.Serializable]
//Guarda datos del desempeño del niño
public class LevelData
{
    public float bestTime = -1f;
    public int bestHits = -1;
    public int bestMistakes = -1;

    public int retries = 0;
    public int stars = 0; // 0–3
}
