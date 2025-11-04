using UnityEngine;

public class SpaceRhythmTimeline : MonoBehaviour
{
    public RectTransform notesRoot;

    [Header("Speed")]
    public float pixelsPerSecond = 200f;

    void Update()
    {
        notesRoot.anchoredPosition +=
            Vector2.down * pixelsPerSecond * Time.deltaTime;
    }
}
