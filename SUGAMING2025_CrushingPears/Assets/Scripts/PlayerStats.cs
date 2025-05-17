using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();   
    if(item != null)
        {
            item.Collect(this);
        }
    }

    public void CollectItem(string itemName, int amount = 1)
    {
        if (inventory.ContainsKey(itemName)) {

            inventory[itemName] += amount;
        }
        else
        {
            inventory[itemName] = amount;
        }

        Debug.Log($"Collected {amount} of {itemName}. Total: {inventory[itemName]}");
    }

    public bool HasItem(string itemName)
    {
        return inventory.ContainsKey(itemName) && inventory[itemName] > 0;
    }

    public bool UseItem(string itemName, int amount = 1)
    {
        if (HasItem(itemName) && inventory[itemName] >= amount)
        {
            inventory[itemName] -= amount;

            if (inventory[itemName] <= 0)
            {
                inventory.Remove(itemName);
            }
            return true;
        }

        return false;
    }

    public int getItemCount(string itemName)
    {

        return inventory.ContainsKey(itemName)?inventory[itemName]:0;
    }

    public bool HasGun { get; private set; }

    public void CollectGun()
    {
        HasGun = true;
       
    }
}
