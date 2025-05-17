using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabbingInteraction : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            Debug.Log("Collided with a ledge");
            transform.GetComponentInParent<PlayerController>().ledge = col;
            transform.GetComponentInParent<PlayerController>().collidingWithGrab = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            Debug.Log("No longer colliding with a ledge");
            transform.GetComponentInParent<PlayerController>().collidingWithGrab = false;
        }
    }
}
