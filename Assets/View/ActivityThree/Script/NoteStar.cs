using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteStar : MonoBehaviour
{
    [Header("UI")]
    public Image bubbleImage;
    public TMP_Text label;
    public Sprite imagenInit;

    Sprite imagenCurrent;
    Color originalLabelColor;

    bool isGhost;
    bool revealed = false;

    public void Setup(NoteData item, bool ghost)
    {
        isGhost = ghost;
        revealed = false;
        imagenCurrent = item.imagenStar;

        bubbleImage.sprite = imagenInit;
        originalLabelColor = label.color;

        label.text = item.noteName;
        label.color = originalLabelColor;
        label.enabled = true;
    }

    // Se usa en autoplay y cuando es el turno del niño
    public void ShowColor()
    {

        if (label.text == "?" && !revealed) return;
        bubbleImage.sprite = imagenCurrent;
    }

    public void RevealGhost(string noteName)
    {
        if (!isGhost || revealed) return;

        revealed = true;
        label.text = noteName;
        label.color = originalLabelColor;
        bubbleImage.sprite = imagenCurrent;
    }

    public void ResetToInitial()
    {
        bubbleImage.sprite = imagenInit;
        revealed = false;

        label.enabled = true;
        label.color = originalLabelColor;
        /* label.text = ""; */
    }


    public void SetGhostVisible(bool visible)
    {
        if (!isGhost) return;

        if (visible)
        {
            Debug.Log("Se esta escribiendo el gost");
            revealed = false;
            label.text = "?";
            label.color = Color.black;
            label.enabled = true;
            bubbleImage.sprite = imagenInit;
        }
        else
        {
            label.enabled = false;
        }
    }

}
