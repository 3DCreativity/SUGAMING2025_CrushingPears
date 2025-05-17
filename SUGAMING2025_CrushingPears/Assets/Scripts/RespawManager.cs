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
        StartCoroutine(RespawnCoroutine(player));
    }

    private IEnumerator RespawnCoroutine(GameObject player)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (player != null)
        {
            player.transform.position = currentRespawnPoint;
            player.GetComponent<PlayerController>().EnablePlayer();
        }
    }
}
