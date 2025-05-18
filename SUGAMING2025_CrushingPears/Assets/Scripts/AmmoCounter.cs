using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AmmoCounter : MonoBehaviour
{
    public static AmmoCounter Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI ammoText;
    public string displayPrefix = "Anchors: ";

    


    private int _ammo;
    public int ammo
    {
        get => _ammo;
        set
        {
            _ammo = Mathf.Max(0, value); // Prevent negative ammo
            UpdateDisplay();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = displayPrefix + ammo.ToString();
        }
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
    }

    public void UseAmmo()
    {
        ammo--;
    }
}
