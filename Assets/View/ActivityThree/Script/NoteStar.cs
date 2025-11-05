using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteStar : MonoBehaviour
{
    [Header("UI")]
    public Image bubbleImage;
    public TMP_Text label;
    public Sprite imagenInit;   // estrella gris
    public bool isPending = false; //para el nivel de ritmo
    public bool IsPending { get; private set; }

    public TimedNote timedNote;

    // 🔹 Data de la nota
    public NoteData noteData;

    bool isGhost;
    bool revealed = false;
    bool isActive = false;

    // --------------------------------------------------
    public void Setup(NoteData item, bool ghost)
    {
        noteData = item;
        isGhost = ghost;
        revealed = false;

        label.text = noteData.noteName;
        SetInactive();
    }

    

    // --------------------------------------------------
    // ⭐ Estado normal: bajando
    public void SetInactive()
    {
        isActive = false;
        isPending = false;

        bubbleImage.sprite = imagenInit;
        bubbleImage.color = Color.white;
        label.color = noteData.color;
        label.enabled = true;
    }

    // --------------------------------------------------
    // ⭐ Estado activo: en HitZone
    public void SetActive()
    {
        isActive = true;
        isPending = true;

        bubbleImage.sprite = noteData.imagenStar;
        bubbleImage.color = Color.white;
        label.color = Color.white;
    }

    // --------------------------------------------------
    // 🔁 Llamado desde HitZoneDetector
    public void SetActive(bool active)
    {
        if (active && !isActive)
            SetActive();
        else if (!active && isActive)
            SetInactive();
    }

    // --------------------------------------------------
    // EXISTENTE (ghost / otros modos)
    public void ShowColor()
    {
        if (label.text == "?" && !revealed) return;
        bubbleImage.sprite = noteData.imagenStar;
    }

    public void RevealGhost(string noteName)
    {
        if (!isGhost || revealed) return;

        revealed = true;
        label.text = noteName;
        label.color = Color.white;
        bubbleImage.sprite = noteData.imagenStar;
    }

    public void ResetToInitial()
    {
        SetInactive();
        revealed = false;
    }

    public void SetGhostVisible(bool visible)
    {
        if (!isGhost) return;

        if (visible)
        {
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

    public void EnterHitZone()
    {
        IsPending = true;
        SetActive();
    }

    public void ExitHitZone()
    {
        IsPending = false;
        SetInactive();
    }

    public void Consume()
    {
        IsPending = false;
        Destroy(gameObject);
    }
}
