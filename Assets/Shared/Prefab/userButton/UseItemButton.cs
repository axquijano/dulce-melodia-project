using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Boton de cada niño que se registro en el sistema y que se muestra en el listado de usuarios
public class UseItemButton : MonoBehaviour
{
    // Texto que muestra el nombre del usuario
    public TMP_Text label;

    // Nombre del usuario asignado a este botón
    private string username;

    // Referencia al controlador que maneja la selección de usuarios
    private UserSelectUI controller;

    // Configura el botón con el nombre y el controlador
    public void Setup(string name, UserSelectUI ctrl)
    {
        username = name;
        controller = ctrl;
        label.text = name; // Actualiza el texto visible
    }

    // Llamado cuando se hace clic en el botón
    public void OnClick()
    {
        controller.SelectUser(username); // Notifica la selección
    }
}
