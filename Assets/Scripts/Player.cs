using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InputManager inputManager;
    public SpriteRenderer[] spriteRenderers; // 0: Up, 1: Down, 2: right, 3: left

    private void Start()
    {
        inputManager.OnDragEnd += HandleDragEnd;
    }

    private void HandleDragEnd(Vector2 dragDir)
    {
        Vector2 normalizedDragDir = dragDir.normalized;
        if (Mathf.Abs(normalizedDragDir.x) > Mathf.Abs(normalizedDragDir.y))
        {
            if (normalizedDragDir.x > 0)
            {
                // Right
                SelectSpriteBasedOnDirection("Right");
            }
            else
            {
                // Left
                SelectSpriteBasedOnDirection("Left");
            }
        }
        else
        {
            if (normalizedDragDir.y > 0)
            {
                // Up
                SelectSpriteBasedOnDirection("Up");
            }
            else
            {
                // Down
                SelectSpriteBasedOnDirection("Down");
            }
        }



    }

    private void SelectSpriteBasedOnDirection(string direction)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = Color.white; // Reset color for all directions
        }
        switch (direction)
        {
            case "Up":
                spriteRenderers[0].color = Color.red;
                break;
            case "Down":
                spriteRenderers[1].color = Color.red;
                break;
            case "Right":
                spriteRenderers[2].color = Color.red;
                break;
            case "Left":
                spriteRenderers[3].color = Color.red;
                break;
            default:
                Debug.LogWarning("Invalid direction: " + direction);
                break;

        }
    }

}