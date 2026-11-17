using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneManager : MonoBehaviour
{
    // Usamos const para evitar el uso de 'strings' sueltos en el código (evita typos)
    public const string SCENE_MENU = "Menu";
    public const string SCENE_GAMEPLAY = "Gameplay";

    private static SimpleSceneManager _instance;

    private void Awake()
    {
        // Aseguramos que solo exista un manager y no se destruya entre cambios de escena
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Carga la escena especificada de manera asíncrona.
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        if (_instance != null)
        {
            _instance.StartCoroutine(_instance.LoadSceneAsyncRoutine(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// Métodos directos para vincular a botones de UI (UnityEvent OnClick)
    /// </summary>
    public void GoToGameplay() => LoadScene(SCENE_GAMEPLAY);
    public void GoToMenu() => LoadScene(SCENE_MENU);

    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        // Opcional: Si tienes una pantalla o Canvas de Fade Out / Loading, lo activas aquí.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Mantiene la corrutina hasta que la escena termine de cargarse en memoria
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Opcional: Limpieza explícita de memoria no utilizada al cambiar de escena
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }
}