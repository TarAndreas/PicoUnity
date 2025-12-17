using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    Vector3 lastVelocity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WhiteBall")
        {
            //var rb = other.GetComponent<Rigidbody>();
            //var speed = lastVelocity.magnitude * 50;
            //var direction = Vector3.Reflect(lastVelocity.normalized, rb.velocity);
            //rb.velocity = direction * Mathf.Max(speed, 0f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "WhiteBall")
        {
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.linearVelocity = (collision.relativeVelocity * 2.5f);
        }

    }
}
