using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "StudentAvatarDatabase",
    menuName = "DulceMelodia/Avatar Database"
)]
public class StudentAvatarDatabase : ScriptableObject
{
    public List<StudentAvatarData> avatars;

    public StudentAvatarData GetById(string id)
    {
        return avatars.Find(a => a.avatarName == id);
    }
}
