using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [Header("Has The Gun")]
    public bool hasGun = false;

    [Header("Aiming Settings")]
    public float rotationSpeed = 10f;
    public float maxUpAngle = 80f;
    public float maxDownAngle = -8f;

    [Header("References")]
    public PlayerController playerController;
    private Camera mainCamera;
    private Vector3 originalScale;
    public GameObject bullet;
    public Transform bulletTransform;

    public int amountBullets = 0;
    public bool canFire;
    private float timer;
    public float timeBetweenShots;

    void Start()
    {
        mainCamera = Camera.main;
        originalScale = transform.localScale;

        if (playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        // Get mouse position in world space
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);

        // Calculate direction to mouse
        Vector3 direction = mouseWorldPos - transform.position;
        float rawAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust angle based on facing direction
        float finalAngle = playerController.m_FacingRight
            ? rawAngle
            : 180 - rawAngle;  // Mirror the angle when facing left

        // Clamp angle (same range for both directions)
        finalAngle = Mathf.Clamp(finalAngle, maxDownAngle, maxUpAngle);

        // Apply rotation
        Quaternion targetRotation = Quaternion.Euler(0, 0, finalAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Handle scale flipping
        transform.localScale = playerController.m_FacingRight
            ? originalScale
            : new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    public void AddAmmo(int amountToAdd)
    {
        amountBullets += amountToAdd;
        if (AmmoCounter.Instance != null)
        {
            AmmoCounter.Instance.AddAmmo(amountToAdd);
        }
        else
        {
            Debug.LogWarning("AmmoCounter instance not found!");
        }

        Debug.Log($"Added {amountToAdd} bullets. Total: {amountBullets}");
    }

    private void HandleShooting()
    {
        

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenShots)
            {
                canFire = true;
                timer = 0;
            }
        }
        if (amountBullets > 0)
        {
            if (Input.GetMouseButtonDown(0) && canFire)
            {
                canFire = false;
                Instantiate(bullet, bulletTransform.position, bulletTransform.rotation);
                amountBullets--;
                if (AmmoCounter.Instance != null)
                {
                    AmmoCounter.Instance.UseAmmo();
                }
                else
                {
                    Debug.LogWarning("AmmoCounter instance not found!");
                }
            }
        }
    }

  
}