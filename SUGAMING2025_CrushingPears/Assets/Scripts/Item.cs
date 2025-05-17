using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private string itemName = "AmmoHook";
    [SerializeField] private int amount = 5;

    public GameObject collectEffect;

    public void Collect(PlayerStats playerStatus)
    {
        ShootingScript shootingScript = playerStatus.GetComponentInChildren<ShootingScript>();

        if (shootingScript != null)
        {
            shootingScript.AddAmmo(amount);
        }

        playerStatus.CollectItem(itemName);

        if (collectEffect != null) { 
        
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        
        }
        Destroy(gameObject);
    }
  
}
