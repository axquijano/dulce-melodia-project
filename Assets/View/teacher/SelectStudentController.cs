using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectStudentController : MonoBehaviour
{
    [Header("UI")]
    public Transform listContainer;
    public GameObject studentItemPrefab;
    public GameObject emptyStatePanel;

    [Header("Scroll")]
    public ScrollRect listScroll;

    void Start()
    {
        LoadStudents();
        StartCoroutine(ScrollToTopCo()); 
    }

    void LoadStudents()
    {
        var profiles = ProfilesManager.Instance.GetAllProfiles();

        if (profiles == null || profiles.Count == 0)
        {
            emptyStatePanel.SetActive(true);
            return;
        }

        emptyStatePanel.SetActive(false);

        foreach (var profile in profiles)
        {
            GameObject item = Instantiate(studentItemPrefab, listContainer);
            item.GetComponent<StudentItemUI>()
                .Setup(profile.childName, this);
        }
    }

    public void SelectStudent(string name)
    {
        ProfilesManager.Instance.SetCurrentProfile(name);
        SceneLoader.Instance.LoadScene("StudentStatsScene");
    }

    System.Collections.IEnumerator ScrollToTopCo()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        listScroll.verticalNormalizedPosition = 1f; // Mostrar el principio del contenido del scroll
    }
    public void Back()
    {
        SceneLoader.Instance.LoadScene("TeacherMenu");
    }
}
