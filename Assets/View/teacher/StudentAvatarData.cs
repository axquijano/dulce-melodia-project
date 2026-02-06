using UnityEngine;

[CreateAssetMenu(
    fileName = "NewStudentAvatar",
    menuName = "DulceMelodia/Student Avatar"
)]
public class StudentAvatarData : ScriptableObject
{
    public string avatarName;
    public Sprite avatarSprite;

    [Header("Estimulación")]
    public Sprite happySprite;
    public Sprite sadSprite;
    public Sprite celebrationSprite;

    [Header("Colores")]
    public Color mainColor;
}
