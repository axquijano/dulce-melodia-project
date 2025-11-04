using UnityEngine;
using UnityEngine.UI;

public class StarNoteUI : MonoBehaviour
{
    public NoteData noteData;
    public Image glow;

    public RectTransform Rect { get; private set; }

    void Awake()
    {
        Rect = GetComponent<RectTransform>();
        glow.enabled = false; 
    }

    public void SetActive(bool active)
    {
        glow.enabled = active;
    }
}
