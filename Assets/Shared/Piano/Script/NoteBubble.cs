using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteBubble : MonoBehaviour
{
    public Image bubbleImage;
    public TMP_Text label;
    public Sprite imagenInit;
    private Sprite imagenCurrent;
    private Outline outline;

    public void Setup(NoteData data)
    {
        bubbleImage.sprite = imagenInit;
        imagenCurrent = data.imagen;
        label.text = data.noteName;
    }

    void Awake()
    {
        // Busca o agrega el Outline
        outline = bubbleImage.GetComponent<Outline>();
        if (outline == null)
            outline = bubbleImage.gameObject.AddComponent<Outline>();

        outline.enabled = false; // apagado por defecto
    }

    public void SetImagenColor (){
        bubbleImage.sprite = imagenCurrent;
    }

    public void Highlight(bool state)
    {
        if(state) SetImagenColor();
       /*  outline.enabled = state; */
    }
}
