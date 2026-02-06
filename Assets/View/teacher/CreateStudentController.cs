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

    [Header("Avatar Grid")]
    public Transform avatarGrid;
    public GameObject avatarItemPrefab;
    public List<StudentAvatarData> avatars;

    [Header("Message")]
    public TMP_Text msgWarning;
    // Avatar seleccionado
    private StudentAvatarData selectedAvatar;

    // Referencias a los items del grid (para marcar selección)
    //TODO : usar la base de datos 
    private List<AvatarItemUI> avatarItems = new List<AvatarItemUI>();

    void Start()
    {
        LoadAvatars();
        previewNameText.text = "";
        previewAvatarImage.sprite = null;
    }

    // Se llama cuando escriben el nombre
    public void OnNameChanged()
    {
        previewNameText.text = nameInput.text;
        msgWarning.text = "";
    }

    // Cargar avatares dinámicamente en el grid
    void LoadAvatars()
    {
        foreach (var avatar in avatars)
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
            Debug.Log("Falta nombre o avatar");
            msgWarning.text = "Falta nombre o avatar";
            return;
        }

        ProfilesManager.Instance.CreateProfile( nameInput.text,selectedAvatar );

        //TODO: revisar esta linea para despues 
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
