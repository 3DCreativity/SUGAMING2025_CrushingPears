using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallDeath : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float deadlyFallVelocity = -15f;

    [Header("Effects")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private AudioClip deathSound;

    private Rigidbody2D rb;
    private PlayerController playerController;
    private float maxFallVelocity;
    private bool wasFalling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        if (RespawManager.Instance == null)
        {
            GameObject respawnManager = new GameObject("RespawnManager");
            respawnManager.AddComponent<RespawManager>();
            respawnManager.GetComponent<RespawManager>().defaultRespawnPoint = transform.position;
        }
    }

    void FixedUpdate()
    {
        if (!playerController.isGrounded)
        {
            if (rb.velocity.y < maxFallVelocity)
            {
                maxFallVelocity = rb.velocity.y;
            }
        }
        else if (wasFalling && maxFallVelocity <= deadlyFallVelocity)
        {
            DieFromFall();
        }

        wasFalling = !playerController.isGrounded;
        if (playerController.isGrounded) maxFallVelocity = 0;
    }

    private void DieFromFall()
    {
        Debug.Log("Player died from fall!");

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);
        if (deathSound != null) AudioSource.PlayClipAtPoint(deathSound, transform.position);

        playerController.DisablePlayer();
        RespawManager.Instance.RespawnPlayer(gameObject);
    }
}
