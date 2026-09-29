using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Clase base para todas las ventanas de UI en la escena.
/// Proporciona funcionalidad para mostrar y ocultar ventanas con animaciones.
/// </summary>
public class UIWindow : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string _id;
    [Header("UI Settings")]
    [SerializeField] private RectTransform _canvasRectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _hideOnStart;

    [Header("Animation Settings")]
    [SerializeField] private float showDuration = 0.5f;
    [SerializeField] private float hideDuration = 0.5f;

    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;

    public CanvasGroup CanvasGroup => _canvasGroup;
    public RectTransform CanvasRectTransform => _canvasRectTransform;
    public string Id => _id;

    void Start()
    {
        //Initialize corre en el Start para asegurarse de que la ventana se inicialice correctamente al inicio del juego.
        Initialize();
    }

    /// <summary>
    /// Inicializa la ventana de UI. Si _hideOnStart es verdadero, la ventana se ocultará al inicio.
    /// </summary>
    public virtual void Initialize()
    {
        if (_hideOnStart)
        {
            Hide(true);
        }
    }
    /// <summary>
    /// Muestra la ventana de UI.
    /// </summary>
    /// <param name="instant">Si es true, la ventana se mostrará instantáneamente.</param>
    public virtual void Show(bool instant = false)
    {
        // Si "instant" es verdadero, se activa el GameObject del Canvas y se muestra la ventana sin animación.
        if (instant)
        {
            // Activar el GameObject del Canvas y mostrar la ventana sin animación
            _canvasRectTransform.gameObject.SetActive(true);
        }
        // Si "instant" es falso, se activa el GameObject del Canvas y se muestra la ventana con animación.
        else
        {
            _canvasRectTransform.gameObject.SetActive(true);
            RectTransform rectTransform = _canvasGroup.GetComponent<RectTransform>();
            rectTransform.DOScale(Vector3.one, showDuration).SetEase(showEase);
        }
    }

    /// <summary>
    /// Oculta la ventana de UI.
    /// </summary>
    /// <param name="instant"> Si es true, la ventana se ocultará instantáneamente.</param>
    public virtual void Hide(bool instant = false)
    {
        // Si "instant" es verdadero, se desactiva el GameObject del Canvas y se oculta la ventana sin animación.
        if (instant)
        {
            _canvasRectTransform.gameObject.SetActive(false);
        }
        // Si "instant" es falso, se oculta la ventana con animación y luego se desactiva el GameObject del Canvas.
        else
        {
            RectTransform rectTransform = _canvasGroup.GetComponent<RectTransform>();
            rectTransform.DOScale(Vector3.zero, hideDuration).SetEase(hideEase).OnComplete(() =>
            {
                _canvasRectTransform.gameObject.SetActive(false);
            });
        }
    }


}