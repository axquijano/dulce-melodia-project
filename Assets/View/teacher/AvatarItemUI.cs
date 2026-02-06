using UnityEngine;
using UnityEngine.UI;

public class AvatarItemUI : MonoBehaviour
{
    public Image avatarImage;

    private StudentAvatarData avatarData;
    private CreateStudentController controller;

    public StudentAvatarData AvatarData => avatarData;

    // Colores
    private Color normalColor = Color.white;
    private Color selectedColor = Color.gray;

    public void Setup(StudentAvatarData data, CreateStudentController ctrl)
    {
        avatarData = data;
        controller = ctrl;

        avatarImage.sprite = data.avatarSprite;
        avatarImage.color = normalColor; // inicia normal
    }

    public void OnClick()
    {
        controller.SelectAvatar(avatarData);
    }

    // Llamado desde CreateStudentController
    public void SetSelected(bool value)
    {
        avatarImage.color = value ? selectedColor : normalColor;
    }
}
