using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    public Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    private bool isStuck = false;
    public LayerMask groundLayer;
    public float stickOffset = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x);
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is on the Ground layer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            StickToSurface(collision);
        }
    }

    private void StickToSurface(Collision2D collision)
    {
        isStuck = true;

        // Stop all movement
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        // Get the contact point and normal
        ContactPoint2D contact = collision.contacts[0];
        Vector2 surfaceNormal = contact.normal;
        Vector2 contactPoint = contact.point;

        // Calculate offset position
        Vector2 offsetPosition = contactPoint + surfaceNormal * stickOffset;

        // Set new position with offset
        transform.position = offsetPosition;

        // Rotate to align with surface normal (optional)
        float angle = Mathf.Atan2(surfaceNormal.y, surfaceNormal.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Parent to the surface if you want it to move with the platform
        transform.SetParent(collision.transform);
    }
    // Update is called once per frame
    void Update()
    {

    }
}