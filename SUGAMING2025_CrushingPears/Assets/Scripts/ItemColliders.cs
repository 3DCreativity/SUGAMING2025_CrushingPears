using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ItemCollider : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField] private bool isTrigger = true; // Set this in Inspector to match your collider type
    [SerializeField] private string playerTag = "Player";

    [Header("Item Stability")]
    [SerializeField] private bool freezePosition = true; // Prevents falling
    [SerializeField] private bool freezeRotation = true; // Prevents spinning

    private void Start()
    {
        ConfigureRigidbody();
    }

    private void ConfigureRigidbody()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Freeze movement if needed
        if (freezePosition)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        // Freeze rotation if needed
        if (freezeRotation)
        {
            rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        }

        // Ensure proper collision detection
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.gravityScale = 0; // No gravity
    }

    // For Trigger Collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTrigger && other.CompareTag(playerTag))
        {
            HandlePlayerCollision(other.GetComponent<PlayerStats>());
        }
    }

    // For Non-Trigger Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTrigger && collision.gameObject.CompareTag(playerTag))
        {
            HandlePlayerCollision(collision.gameObject.GetComponent<PlayerStats>());
        }
    }

    protected virtual void HandlePlayerCollision(PlayerStats playerStats)
    {
        // Override this in child classes for specific item behavior
        Debug.Log($"Player collected {gameObject.name}");
    }
}
