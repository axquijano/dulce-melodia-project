using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UserSelectUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField nameField;
    public Transform listContainer;
    public GameObject userButtonPrefab;

    IEnumerator RefreshListCo()
    {
        // Destruir objetos en el próximo frame
        yield return null;

        foreach (Transform t in listContainer)
            Destroy(t.gameObject);

        yield return null;

        var profiles = ProfilesManager.Instance.GetAllProfiles();

        foreach (var p in profiles)
        {
            GameObject btn = Instantiate(userButtonPrefab, listContainer);
            btn.GetComponent<UseItemButton>().Setup(p.childName, this);
        }
        

    }

    void Start()
    {
        StartCoroutine(RefreshListCo());
    }

    public void CreateNewUser()
    {
        if (string.IsNullOrEmpty(nameField.text))
            return;

        ProfilesManager.Instance.CreateProfile(nameField.text);
        ProfilesManager.Instance.SetCurrentProfile(nameField.text);

        SceneLoader.Instance.LoadScene("MapActivity");
    }

    public void SelectUser(string name)
    {
        ProfilesManager.Instance.SetCurrentProfile(name);
        SceneLoader.Instance.LoadScene("MapActivity");
    }

    public void Back()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
