using UnityEngine;

public class ClawController : MonoBehaviour
{
    [Header("Configuración de Péndulo (Liana)")]
    [Tooltip("Velocidad de oscilación de la liana.")]
    [SerializeField] private float swingSpeed = 2.0f;

    [Tooltip("Ángulo máximo de inclinación a los lados (en grados).")]
    [SerializeField] private float maxAngle = 25.0f;

    [Tooltip("Longitud virtual de la liana desde su punto de origen arriba.")]
    [SerializeField] private float ropeLength = 4.5f;

    [Header("Físicas de Impulso")]
    [Tooltip("Multiplicador del impulso horizontal transferido al bloque (1.0 = inercia real).")]
    [SerializeField] private float momentumMultiplier = 1.0f;

    public Vector2 CurrentVelocity { get; private set; }

    private Vector3 anchorPoint;
    private Vector3 lastPosition;
    private bool isSwinging = true;

    private void Awake()
    {
        anchorPoint = transform.position + Vector3.up * ropeLength;
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (!isSwinging) return;

        // 1. Movimiento del péndulo (nunca se frena)
        float currentAngle = Mathf.Sin(Time.time * swingSpeed) * maxAngle;
        float angleRad = currentAngle * Mathf.Deg2Rad;

        float xOffset = ropeLength * Mathf.Sin(angleRad);
        float yOffset = -ropeLength * Mathf.Cos(angleRad);

        transform.position = anchorPoint + new Vector3(xOffset, yOffset, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, -currentAngle);

        // 2. Cálculo frame a frame de la velocidad vectorial instantánea
        if (Time.deltaTime > 0f)
        {
            Vector2 frameVelocity = (transform.position - lastPosition) / Time.deltaTime;
            CurrentVelocity = frameVelocity * momentumMultiplier;
        }

        lastPosition = transform.position;
    }

    public void SetSwinging(bool active)
    {
        isSwinging = active;
    }

    public void IncreaseDifficulty(float speedIncrement)
    {
        swingSpeed += speedIncrement;
    }

    public void UpdateAnchorY(float deltaY)
    {
        anchorPoint += Vector3.up * deltaY;
    }
}