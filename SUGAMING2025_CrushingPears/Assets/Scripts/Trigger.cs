using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{

    [SerializeField] private Collider2D collider;
    [SerializeField] private string key;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider == collider)
        {
            //Lock Player
            DialogueManager.Instance.StartDialogue(key);
        }
    }
}
