using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameplayManager : MonoBehaviour
{
    public enum GameState { Aiming, Dropping, GameOver }

    [Header("Referencias de la Escena")]
    [SerializeField] private ClawController clawController;
    [SerializeField] private Transform blockSpawnPoint;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private LoseLife loseLifeZone;
    [SerializeField] private TMPro.TextMeshProUGUI livesText;

    [Header("Ajustes de Juego")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private float speedIncrementPerRound = 0.15f;
    [SerializeField] private bool ignoreTapsOverUI = true;
    [SerializeField] private CameraController cameraController;

    public event Action OnGameOver;
    public GameState State { get; private set; } = GameState.Aiming;

    private BlockFreezer currentBlock;
    private int livesRemaining;
    private bool tapRequested;
    private bool isSubscribed;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTapEvent += HandleTap;
            isSubscribed = true;
        }

        if (loseLifeZone != null)
        {
            loseLifeZone.OnBlockFallen += HandleBlockLost;
        }

        livesRemaining = startingLives;
        UpdateLivesUI();
        SpawnNewBlock();
    }

    private void OnDestroy()
    {
        if (isSubscribed && InputManager.Instance != null)
            InputManager.Instance.OnTapEvent -= HandleTap;

        if (loseLifeZone != null)
            loseLifeZone.OnBlockFallen -= HandleBlockLost;

        UnsubscribeCurrentBlock();
    }

    private void HandleTap(bool pressed)
    {
        if (!pressed || State != GameState.Aiming) return;

        if (ignoreTapsOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        tapRequested = true;
    }

    private void Update()
    {
        if (State == GameState.Aiming && tapRequested)
        {
            tapRequested = false;
            DropCurrentBlock();
        }
    }

    private void DropCurrentBlock()
    {
        if (currentBlock == null) return;

        State = GameState.Dropping;

        // Obtener el vector de velocidad actual de la garra y pasárselo al bloque
        Vector2 clawVelocity = (clawController != null) ? clawController.CurrentVelocity : Vector2.zero;
        currentBlock.ReleaseBlock(clawVelocity);
    }

    private void SpawnNewBlock()
    {
        State = GameState.Aiming;
        tapRequested = false;

        GameObject blockGO = Instantiate(blockPrefab, blockSpawnPoint.position, Quaternion.identity, blockSpawnPoint);

        // Asignación de color aleatorio
        if (blockGO.TryGetComponent<SpriteRenderer>(out var sr))
        {
            sr.color = UnityEngine.Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.8f, 1f);
        }

        currentBlock = blockGO.GetComponent<BlockFreezer>();
        if (currentBlock != null)
        {
            currentBlock.OnFrozen += HandleBlockFrozen;
        }
    }

    private void HandleBlockFrozen(BlockFreezer frozenBlock)
    {
        UnsubscribeCurrentBlock();
        clawController.IncreaseDifficulty(speedIncrementPerRound);

        // Verificar si la torre alcanzó la mitad de la pantalla para subir la vista
        if (cameraController != null)
        {
            cameraController.CheckBlockHeight(frozenBlock.transform.position.y, out float deltaY);
            if (deltaY > 0f)
            {
                // Elevar la garra en conjunto con la cámara para mantener la distancia de caída
                clawController.UpdateAnchorY(deltaY);
            }
        }

        SpawnNewBlock();
    }

    private void HandleBlockLost(GameObject fallenBlock)
    {
        if (State == GameState.GameOver) return;

        UnsubscribeCurrentBlock();
        livesRemaining = Mathf.Max(livesRemaining - 1, 0);
        UpdateLivesUI();

        if (livesRemaining == 0)
        {
            ChangeState(GameState.GameOver);
        }
        else
        {
            SpawnNewBlock();
        }
    }

    private void ChangeState(GameState newState)
    {
        State = newState;

        if (newState == GameState.GameOver)
        {
            clawController.SetSwinging(false);
            OnGameOver?.Invoke();
        }
    }

    private void UnsubscribeCurrentBlock()
    {
        if (currentBlock != null)
        {
            currentBlock.OnFrozen -= HandleBlockFrozen;
            currentBlock = null;
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = $"{livesRemaining}";
        }
    }
}