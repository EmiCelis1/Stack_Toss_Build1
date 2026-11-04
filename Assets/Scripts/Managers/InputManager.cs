using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private float minDragDistance = 20f; // píxeles

    #region Drag Direction
    public event Action<Vector2> OnDragEnd;

    private Vector2 _startPosition;
    private Vector2 _currentPosition;
    private bool _isDragging;

    private InputAction _contactAction;
    private InputAction _positionAction;
    #endregion

    #region Tap
    public event Action<bool> OnTapEvent;
    #endregion

    #region Continuous
    public event Action<Vector2> OnContinuousEvent;
    #endregion

    private PlayerInput _playerInput;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        if (Instance != this) return;

        var dragMap = _playerInput.actions.FindActionMap("Drag", throwIfNotFound: true);
        dragMap.Enable();

        _contactAction = dragMap.FindAction("PrimaryContact", throwIfNotFound: true);
        _positionAction = dragMap.FindAction("PrimaryPosition", throwIfNotFound: true);

        _contactAction.started += OnTouchStart;
        _contactAction.canceled += OnTouchEnd;
        _positionAction.performed += OnPositionChanged;
    }

    private void OnDisable()
    {
        if (_contactAction != null)
        {
            _contactAction.started -= OnTouchStart;
            _contactAction.canceled -= OnTouchEnd;
        }

        if (_positionAction != null)
            _positionAction.performed -= OnPositionChanged;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    #region Drag Direction
    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        _isDragging = true;
        _startPosition = _positionAction.ReadValue<Vector2>();
        _currentPosition = _startPosition;
    }

    private void OnPositionChanged(InputAction.CallbackContext ctx)
    {
        if (_isDragging)
            _currentPosition = ctx.ReadValue<Vector2>();
    }

    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        if (!_isDragging) return;
        _isDragging = false;

        Vector2 delta = _currentPosition - _startPosition;
        if (delta.sqrMagnitude < minDragDistance * minDragDistance) return;

        OnDragEnd?.Invoke(delta.normalized);
    }
    #endregion

    #region Tap
    public void OnTap(InputAction.CallbackContext context)
    {
        if (context.started)
            OnTapEvent?.Invoke(true);
        else if (context.canceled)
            OnTapEvent?.Invoke(false);
    }
    #endregion

    #region Continuous
    public void OnContinuous(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
            OnContinuousEvent?.Invoke(context.ReadValue<Vector2>());
    }
    #endregion
}