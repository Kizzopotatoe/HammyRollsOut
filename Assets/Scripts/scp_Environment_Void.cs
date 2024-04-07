using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class scp_Environment_Void : MonoBehaviour
{
    public Transform checkpoint;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            other.transform.position = checkpoint.transform.position; 
        }
    }
}
