using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PianoKey : MonoBehaviour
{
    public NoteData noteData;
    private AudioSource audioSource;

    public bool allowInternalSound = true; // para el nivel de ritmo, el sonido se controla desde el manager

    public event Action<NoteData> onKeyPressed;

    [Header("UI References")]
    public Button keyButton;
    public Image guideCircle;

    private Coroutine blinkRoutine;

    private Sprite idleSprite;
    private Sprite highlightedSprite;
    private Sprite pressedSprite;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // Guardamos los sprites del SpriteState
        idleSprite = keyButton.image.sprite;
        pressedSprite = keyButton.spriteState.pressedSprite;
    
        if (guideCircle != null)
            guideCircle.gameObject.SetActive(false);
        
    
    }

    public void PlayNote()
    {
        if (!keyButton.interactable) return;

        if (allowInternalSound && noteData.sound != null)
        {
            audioSource.PlayOneShot(noteData.sound);
        }

        onKeyPressed?.Invoke(noteData);
        ResetVisualHelp();
    }

    public void PlayExternalSound(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
    }


    public void SetKeyEnabled(bool enabled)
    {
        keyButton.interactable = enabled;
        keyButton.image.color = Color.white;

    }

    public void ShowHelp()
    {
        // Activar círculo guía
        if (guideCircle != null)
            guideCircle.gameObject.SetActive(true);

        // Iniciar parpadeo
        if (blinkRoutine != null) StopCoroutine(blinkRoutine);
        blinkRoutine = StartCoroutine(BlinkSprites());
    }

    IEnumerator BlinkSprites()
    {
        float speed = 0.35f;

        while (true)
        {
            // Sprite 2 → Idle
            keyButton.image.sprite = idleSprite;
            yield return new WaitForSeconds(speed);

            // Puedes agregar una tercera fase:
             keyButton.image.sprite = pressedSprite;
             yield return new WaitForSeconds(speed);
        }
    }

    public void ResetVisualHelp()
    {
        // detener parpadeo
        if (blinkRoutine != null) StopCoroutine(blinkRoutine);
        blinkRoutine = null;

        // restaurar sprite original
        keyButton.image.sprite = idleSprite;

        // ocultar círculo guía
        if (guideCircle != null)
            guideCircle.gameObject.SetActive(false);
    }
}
