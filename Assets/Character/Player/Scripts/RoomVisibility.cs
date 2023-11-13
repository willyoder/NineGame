using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVisibility : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Assuming your object has a SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is touching the trigger zone, hide the object
            spriteRenderer.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        bool isVisible = spriteRenderer.isVisible;
        if (other.CompareTag("Player"))
        {
            if (!isVisible)
            {
                spriteRenderer.enabled = true;
            }
        }
    }
}
