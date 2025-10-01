using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/Level Settings")]
public class LevelSettings : ScriptableObject
{
    [Header("Spawner")]
    public float spawnInterval = 1.5f;

    [Header("Balloon Probabilities (0–1)")]
    [Range(0f, 1f)] public float pctFull = 0.5f;   // color + letra
    [Range(0f, 1f)] public float pctColor = 0.3f;  // solo color
    [Range(0f, 1f)] public float pctLetter = 0.2f; // solo letra

    [Header("Win/Lose Settings")]
    public int balloonsToWin = 10;
    public int allowedMistakes = 3;
}
