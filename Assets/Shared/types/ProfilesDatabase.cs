using System.Collections.Generic;
using System.Collections;

[System.Serializable]
//Almacena todos los perfiles en el archivo JSON.
public class ProfilesDatabase
{
    public List<ChildProfile> profiles = new List<ChildProfile>();
}