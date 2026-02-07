using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StudentItemUI : MonoBehaviour
{
    public TMP_Text nameText;

    public void Setup(string studentName, SelectStudentController controller)
    {
        nameText.text = studentName;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            controller.SelectStudent(studentName);
        });
    }
}
