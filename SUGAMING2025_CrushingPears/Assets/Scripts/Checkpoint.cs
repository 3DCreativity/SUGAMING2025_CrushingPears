using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 spawnOffset = new Vector3(0, 1f, 0);

    private bool isActive;
    private Collider2D triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();

        // Verify setup
        if (triggerCollider == null)
        {
            Debug.LogError("No Collider2D found on checkpoint!", this);
            return;
        }

        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning("Checkpoint collider should be set as Trigger!", this);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger entered with: {other.gameObject.name}");

        if (!isActive && other.CompareTag("Player"))
        {
            Debug.Log("Player entered checkpoint!");
            ActivateCheckpoint();
        }
    }

    void ActivateCheckpoint()
    {
        isActive = true;
        Debug.Log($"Activating checkpoint at {transform.position}");

        // Set respawn point
        Vector3 spawnPoint = transform.position + spawnOffset;
        RespawManager.Instance.SetRespawnPoint(spawnPoint);

       
    }
}
