using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BlockFreezer : MonoBehaviour
{
    [Header("Efecto Caída de Pluma")]
    [Tooltip("Escala de gravedad baja para un descenso flotante.")]
    [SerializeField] private float gravityScale = 0.4f;

    [Tooltip("Alta resistencia del aire para frenar caídas bruscas.")]
    [SerializeField] private float linearDrag = 3.5f;

    [Tooltip("Velocidad máxima de caída hacia abajo (límite de velocidad terminal).")]
    [SerializeField] private float maxFallSpeed = 2.0f;

    [Header("Bamboleo de Pluma en el Aire")]
    [Tooltip("Activa la oscilación de izquierda a derecha al flotar.")]
    [SerializeField] private bool enableFeatherSway = true;

    [Tooltip("Frecuencia con la que se mece la pluma de un lado a otro.")]
    [SerializeField] private float swayFrequency = 4.0f;

    [Tooltip("Intensidad del balanceo horizontal.")]
    [SerializeField] private float swayAmplitude = 0.6f;

    [Header("Físicas y Capas")]
    [SerializeField] private LayerMask surfaceLayerMask;
    [SerializeField] private float freezeDelay = 3.0f;

    public event Action<BlockFreezer> OnFrozen;
    public bool IsFrozen { get; private set; }

    private Rigidbody2D rb;
    private bool isReleased = false;
    private int contactCount = 0;
    private float contactTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearDamping = linearDrag;
        rb.gravityScale = gravityScale;
    }

    public void ReleaseBlock(Vector2 initialVelocity)
    {
        transform.SetParent(null);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = initialVelocity;
        isReleased = true;
    }

    private void FixedUpdate()
    {
        if (!isReleased || IsFrozen) return;

        // 1. Limitar la velocidad de caída vertical para que descienda a ritmo constante (efecto paracaídas/pluma)
        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxFallSpeed);
        }

        // 2. Bamboleo horizontal de pluma mientras flota en el aire (antes de tocar suelo)
        if (enableFeatherSway && contactCount == 0)
        {
            float swayForce = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x + swayForce * Time.fixedDeltaTime, rb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsFrozen) return;

        if (((1 << collision.gameObject.layer) & surfaceLayerMask) != 0)
        {
            contactCount++;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsFrozen) return;

        if (((1 << collision.gameObject.layer) & surfaceLayerMask) != 0)
        {
            contactCount = Mathf.Max(0, contactCount - 1);
            if (contactCount == 0)
            {
                contactTimer = 0f;
            }
        }
    }

    private void Update()
    {
        if (IsFrozen || contactCount == 0) return;

        contactTimer += Time.deltaTime;
        if (contactTimer >= freezeDelay)
        {
            Freeze();
        }
    }

    private void Freeze()
    {
        IsFrozen = true;
        isReleased = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;

        int frozenLayer = LayerMask.NameToLayer("FrozenBlock");
        if (frozenLayer != -1)
        {
            gameObject.layer = frozenLayer;
        }

        OnFrozen?.Invoke(this);
    }
}