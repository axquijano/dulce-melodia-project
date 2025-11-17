using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteBubble : MonoBehaviour
{
    public Image bubbleImage;
    public TMP_Text label;


    public void Setup(NoteData data)
    {
        bubbleImage.color = data.noteColor;
        label.text = data.noteName;
    }

   private Outline outline;  

    void Awake()
    {
        // Busca o agrega el Outline
        outline = bubbleImage.GetComponent<Outline>();
        if (outline == null)
            outline = bubbleImage.gameObject.AddComponent<Outline>();

        outline.enabled = false; // apagado por defecto
    }

    public void Highlight(bool state)
    {
        outline.enabled = state;
    }
}
