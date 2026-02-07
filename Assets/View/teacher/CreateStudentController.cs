using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class CreateStudentController : MonoBehaviour
{
    [Header("Input")]
    public TMP_InputField nameInput;

    [Header("Avatar Preview")]
    public Image previewAvatarImage;
    public TMP_Text previewNameText;

    [Header("Avatar Placeholder")]
    public Sprite defaultAvatarSprite; // silueta del monstruo

    [Header("Avatar Grid")]
    public Transform avatarGrid;
    public GameObject avatarItemPrefab;

    [Header("Avatar Database")]
    public StudentAvatarDatabase avatarDatabase;

    [Header("Message")]
    public TMP_Text msgWarning;

    // Avatar seleccionado
    private StudentAvatarData selectedAvatar;

    // Referencias a los items del grid (selección visual)
    private List<AvatarItemUI> avatarItems = new List<AvatarItemUI>();

    void Start()
    {
        previewNameText.text = "";
        previewAvatarImage.sprite = defaultAvatarSprite;
        selectedAvatar = null;

        LoadAvatars();
    }

    // Se llama cuando escriben el nombre
    public void OnNameChanged()
    {
        previewNameText.text = nameInput.text;
        msgWarning.text = "";
    }

    // Cargar avatares dinámicamente desde la base de datos
    void LoadAvatars()
    {
        // Limpieza por si se recarga la escena
        foreach (Transform t in avatarGrid)
            Destroy(t.gameObject);

        avatarItems.Clear();

        foreach (var avatar in avatarDatabase.avatars)
        {
            GameObject item = Instantiate(avatarItemPrefab, avatarGrid);
            AvatarItemUI ui = item.GetComponent<AvatarItemUI>();

            ui.Setup(avatar, this);
            avatarItems.Add(ui);
        }
    }

    // Se llama desde AvatarItemUI
    public void SelectAvatar(StudentAvatarData avatar)
    {
        selectedAvatar = avatar;
        previewAvatarImage.sprite = avatar.avatarSprite;

        // Actualizar selección visual
        foreach (var item in avatarItems)
        {
            item.SetSelected(item.AvatarData == avatar);
        }

        msgWarning.text = "";
    }

    // Botón Guardar
    public void SaveStudent()
    {
        if (string.IsNullOrEmpty(nameInput.text) || selectedAvatar == null)
        {
            msgWarning.text = "Falta nombre o avatar";
            return;
        }

        ProfilesManager.Instance.CreateProfile(nameInput.text, selectedAvatar);
        ProfilesManager.Instance.SetCurrentProfile(nameInput.text);

        Debug.Log("Estudiante guardado correctamente");

        SceneManager.LoadScene("TeacherMenu");
    }

    // Botón Cancelar
    public void Cancel()
    {
        SceneManager.LoadScene("TeacherMenu");
    }

    // Botón Atrás
    public void GoBack()
    {
        SceneManager.LoadScene("TeacherMenu");
    }
}
