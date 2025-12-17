using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables sticky physics on GameObject
/// </summary>
public class StickyBehaviour : MonoBehaviour
{

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Floor")
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        } 

    }

}
