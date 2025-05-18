using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawManager : MonoBehaviour
{
    public static RespawManager Instance;

    [Header("Settings")]
    public float respawnDelay = 2f;
    public Vector3 defaultRespawnPoint;

    private Vector3 currentRespawnPoint;
    private GameObject currentPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentRespawnPoint = defaultRespawnPoint;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetRespawnPoint(Vector3 newPoint)
    {
        currentRespawnPoint = newPoint;
        Debug.Log($"Respawn point set to: {newPoint}");
    }

    public void RespawnPlayer(GameObject player)
    {
        currentPlayer = player;
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        Debug.Log("Starting respawn process...");

        // Disable player immediately
        if (currentPlayer != null)
        {
            currentPlayer.SetActive(false);
        }

        yield return new WaitForSeconds(respawnDelay);

        if (currentPlayer != null)
        {
            Debug.Log($"Respawning at: {currentRespawnPoint}");

            // Reset position and enable
            currentPlayer.transform.position = currentRespawnPoint;
            currentPlayer.SetActive(true);

            // Reset player components
            PlayerController pc = currentPlayer.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.EnablePlayer();
            }
            else
            {
                Debug.LogError("PlayerController component missing!");
            }
        }
        else
        {
            Debug.LogError("Player reference lost during respawn!");
        }
    }
}
