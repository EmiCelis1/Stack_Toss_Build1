using System;
using UnityEngine;

public class LoseLife : MonoBehaviour
{
    [SerializeField] private string blockTag = "Block";

    public event Action<GameObject> OnBlockFallen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(blockTag)) return;

        GameObject block = collision.attachedRigidbody != null
            ? collision.attachedRigidbody.gameObject
            : collision.gameObject;

        OnBlockFallen?.Invoke(block);
        Destroy(block);
    }
}