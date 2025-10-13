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
    private Color labelOriginalColor;


   /*  public void Setup(NoteData data)
    {
        bubbleImage.sprite = imagenInit;
        imagenCurrent = data.imagen;
        label.text = data.noteName;
    } */

    private LevelNoteItem item;

    public void Setup(LevelNoteItem item)
    {
        this.item = item;

        // Imagen base (vacía)
        bubbleImage.sprite = imagenInit;

        // Imagen de la nota (color real)
        imagenCurrent = item.note.imagen;

        // Texto de la nota
        label.text = item.note.noteName;

        // Ocultar según configuración
        label.enabled = item.showLetter;
        
        labelOriginalColor = label.color;
        // El color (imagenCurrent) solo se usa cuando se llame a SetImagenColor()
        // así que no lo mostramos hasta highlight
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

    /* public void Highlight(bool state)
    {
        if(state) SetImagenColor();
       //  outline.enabled = state; 
    } */

    /* public void Highlight(bool state)
    {
        if (state)
        {
            if (item.showColor)
                SetImagenColor();
            
            label.enabled = item.showLetter;
        }
        else
        {
            // Cuando no está seleccionada vuelve a la imagen base e=ícono vacío
            bubbleImage.sprite = imagenInit;

            // Si no debe mostrar letra fuera del highlight, la apagamos
            label.enabled = item.showLetter;
        }
    } */

    /* public void Highlight(int bubbleIndex, int currentIndex)
    {
        // Ya pasada → mantener color
        if (bubbleIndex < currentIndex)
        {
            bubbleImage.sprite = imagenCurrent;
            label.enabled = item.showLetter;
            return;
        }

        // Actual → mostrar color
        if (bubbleIndex == currentIndex)
        {
            bubbleImage.sprite = imagenCurrent;
            label.enabled = item.showLetter;
            return;
        }

        // Futuras → gris
        bubbleImage.sprite = imagenInit;
        label.enabled = item.showLetter;
    } */

    public void Highlight(int bubbleIndex, int currentIndex)
    {
        bool isPast = bubbleIndex < currentIndex;
        bool isCurrent = bubbleIndex == currentIndex;
        bool isFuture = bubbleIndex > currentIndex;

        // ---- PASADA ----
        if (isPast)
        {
            bubbleImage.sprite = imagenCurrent;

            if (item.showLetter)
            {
                label.enabled = true;
                label.color = labelOriginalColor; // Regresa al color original
            }
            else
            {
                label.enabled = false;
            }

            return;
        }

        // ---- ACTUAL ----
        if (isCurrent)
        {
            if (item.showColor)
            {
                SetImagenColor();
            }
            else
            {
                bubbleImage.sprite = imagenInit;
            }

            if (item.showLetter)
            {
                label.enabled = true;
                if(item.showColor)
                    label.color = Color.white; // Letra blanca si muestra color
                else
                    label.color = item.note.color; // Letra negra cuando es la actual
            }
            else
            {
                label.enabled = false;
            }

            return;
        }

        // ---- FUTURA ----
        if (isFuture)
        {
            bubbleImage.sprite = imagenInit;

            if (item.showLetter)
            {
                label.enabled = true;
                label.color = labelOriginalColor;
            }
            else
            {
                label.enabled = false;
            }

            return;
        }
    }


}
