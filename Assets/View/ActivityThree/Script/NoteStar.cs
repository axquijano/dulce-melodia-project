using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteStar : MonoBehaviour
{
    public Image bubbleImage;
    public TMP_Text label;
    public Sprite imagenInit;

    private Sprite imagenCurrent;
    private Color originalLabelColor;

    /* private bool showColor;
    private bool showLetter; */

    public void Setup(NoteData item)
    {
        imagenCurrent = item.imagenStar;
        label.text = item.noteName;

        /* showColor = item.showColor;
        showLetter = item.showLetter; */

        bubbleImage.sprite = imagenInit;
        /* label.enabled = showLetter; */

        originalLabelColor = label.color;
    }

    public void ShowColor()
    {
        bubbleImage.sprite = imagenCurrent;
    } 

    public void ResetToInitial()
    {
        bubbleImage.sprite = imagenInit;

       /*  if (showLetter)
        {
            label.enabled = true;
            label.color = originalLabelColor;
        }
        else
        {
            label.enabled = false;
        } */
    }

    public void HideLetter()
    {
        label.enabled = false;
    }

    public void ShowAsGhost()
    {
        bubbleImage.sprite = imagenInit; 
        label.enabled = false;
    }

    public void ShowAsCompleted()
    {
        // Mantiene la imagen de la nota (estrella "encendida")
        bubbleImage.sprite = imagenCurrent;

        // Asegura que la letra sea visible
        label.enabled = true;

        // Cambia el color de la letra como refuerzo positivo
        //label.color = Color.green;
    }

}
