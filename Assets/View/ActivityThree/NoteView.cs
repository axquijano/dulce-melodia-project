using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class NoteView : MonoBehaviour
{
    [Header("UI")]
    public Image bubbleImage;
    public TMP_Text label;
    public Sprite defaultSprite;

    protected NoteData noteData;

    public virtual void Init(NoteData data)
    {
        noteData = data;
        label.text = noteData.noteName;
        label.color = noteData.color;
        bubbleImage.sprite = defaultSprite;
    }

    protected void SetSprite(Sprite sprite)
    {
        bubbleImage.sprite = sprite;
    }

    protected void SetLabelColor(Color color)
    {
        label.color = color;
    }
}
