using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<UIWindow> _uiWindow;
    public List<UIWindow> UIWindows => _uiWindow;

    void Start()
    {

    }


    public void ShowWindow(string windowName)
    {
        foreach (var window in _uiWindow)
        {
            if (window.Id == windowName)
            {
                Debug.Log($"Showing window: {windowName}");
                window.Show();
                break;
            }
            Debug.LogError("Window not found: " + windowName);
        }
    }

    public void HideWindow(string windowName)
    {
        foreach (var window in _uiWindow)
        {
            if (window.Id == windowName)
            {
                Debug.Log($"Showing window: {windowName}");
                window.Hide();
                break;
            }
            Debug.LogError("Window not found: " + windowName);
        }
    }

    private void ShowWindowPopup()
    {

    }
}
