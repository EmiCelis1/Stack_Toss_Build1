using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using NaughtyAttributes;

public class UIWindow : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _hideOnStart;


    void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        if (_hideOnStart)
        {
            Hide();
        }
    }

    [Button("Show Window")]
    public virtual void Show(bool instant = false)
    {
        if (instant)
        {
            _canvasRectTransform.gameObject.SetActive(true);
        }
        else
        {
            _canvasRectTransform.gameObject.SetActive(true);
            RectTransform rectTransform = _canvasGroup.GetComponent<RectTransform>();
            rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    [Button("Hide Window")]
    public virtual void Hide(bool instant = false)
    {
        if (instant)
        {
            _canvasRectTransform.gameObject.SetActive(false);
        }
        else
        {
            RectTransform rectTransform = _canvasGroup.GetComponent<RectTransform>();
            rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack);
        }
    }

}
