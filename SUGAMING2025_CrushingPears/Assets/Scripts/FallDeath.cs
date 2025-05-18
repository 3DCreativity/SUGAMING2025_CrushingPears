using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallDeath : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float deadlyFallVelocity = -15f;

    private Rigidbody2D rb;
    private PlayerController playerController;
    private float maxFallVelocity;
    private bool wasFalling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        // Immediate error if components missing
        if (rb == null) Debug.LogError("MISSING Rigidbody2D!", this);
        if (playerController == null) Debug.LogError("MISSING PlayerController!", this);
    }

    void FixedUpdate()
    {
     //   Debug.Log($"Current Y Velocity: {rb.velocity.y}"); // Critical debug line

        bool isFalling = !playerController.isGrounded && rb.velocity.y < 0;

        if (isFalling)
        {
            wasFalling = true;
            if (rb.velocity.y < maxFallVelocity)
                maxFallVelocity = rb.velocity.y;
        }
        else if (wasFalling) // Just landed
        {
            Debug.Log($"Landed with velocity: {maxFallVelocity}");

            if (maxFallVelocity <= deadlyFallVelocity)
            {
                Debug.Log("FATAL FALL DETECTED");
                DieFromFall();
            }
            ResetFallTracking();
        }
    }

    void DieFromFall()
    {
        Debug.Log("DEATH TRIGGERED - Calling Respawn");
        playerController.DisablePlayer();
        RespawManager.Instance.RespawnPlayer(gameObject);
    }

    void ResetFallTracking()
    {
        wasFalling = false;
        maxFallVelocity = 0;
    }
}
