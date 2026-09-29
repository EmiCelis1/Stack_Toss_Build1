using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Clase que gestiona las ventanas de UI en la escena.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Lista de UI en nuestra escena.
    /// </summary>
    [SerializeField] private List<UIWindow> _uiWindows;

    /// <summary>
    /// Obtiene la lista de ventanas de UI.
    /// </summary>
    /// <returns></returns>
    public List<UIWindow> UIWindows => _uiWindows;

    /// <summary>
    /// Muestra la ventana de UI con el nombre especificado.
    /// </summary>
    /// <param name="windowName"></param>
    public void ShowWindow(string windowName)
    {
        foreach (var window in _uiWindows)
        {
            if (window.Id == windowName)
            {
                Debug.Log($"Showing window: {windowName}");
                window.Show();
                break;
            }
            else
            {
                Debug.LogError("Window not found: " + windowName);
            }
        }
    }

    /// <summary>
    /// Oculta la ventana de UI con el nombre especificado.
    /// </summary>
    /// <param name="windowName"></param>
    public void HideWindow(string windowName)
    {
        foreach (var window in _uiWindows)
        {
            if (window.Id == windowName)
            {
                Debug.Log($"Hiding window: {windowName}");
                window.Hide();
                break;
            }
            else
            {
                Debug.LogError("Window not found: " + windowName);
            }
        }
    }
}