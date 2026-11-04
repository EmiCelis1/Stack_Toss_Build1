using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BlockFreezer : MonoBehaviour
{
    [Header("Físicas y Capas")]
    [SerializeField] private LayerMask surfaceLayerMask;
    [SerializeField] private float freezeDelay = 3.0f;
    [SerializeField] private float linearDrag = 0.5f;
    [SerializeField] private float gravityScale = 0.8f;

    public event Action<BlockFreezer> OnFrozen;
    public bool IsFrozen { get; private set; }

    private Rigidbody2D rb;
    private int contactCount = 0;
    private float contactTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearDamping = linearDrag;
        rb.gravityScale = gravityScale;
    }

    // Sobrecarga del método que acepta el vector de inercia
    public void ReleaseBlock(Vector2 initialVelocity)
    {
        transform.SetParent(null);
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Transfiere el impulso de la garra al cuerpo rígido
        rb.linearVelocity = initialVelocity;
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