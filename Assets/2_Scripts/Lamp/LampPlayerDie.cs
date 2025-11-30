using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LampPlayerDie : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HIT");
            CharacterController character = other.gameObject.GetComponent<CharacterController>();
            
            character.enabled = false;
            other.transform.position = new Vector3(17, 15.15f, 0);
            character.enabled = true;
        }
    }
}
