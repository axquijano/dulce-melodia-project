using UnityEngine;

public class SpaceRhythmTimeline : MonoBehaviour
{
    [Header("References")]
    public RectTransform notesRoot;
    public AudioSource audioSource;

    [Header("Visual Speed")]
    public float pixelsPerSecond = 200f;

    void Update()
    {
        if (!audioSource || !audioSource.isPlaying)
            return;

        float songTime = audioSource.time;

        // ⬇️ mueve TODA la línea de tiempo
        notesRoot.anchoredPosition =
            new Vector2(0, -songTime * pixelsPerSecond);
    }
}
