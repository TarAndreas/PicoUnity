using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pocket : MonoBehaviour
{
    Vector3 po;
    Quaternion ro;
    private void Start()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.ToLower().Contains("NormalBall".ToLower()))
        {
            collision.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower().Contains("NormalBall".ToLower()))
        {
            other.gameObject.SetActive(false);
        }
        if (other.name.ToLower().Contains("WhiteBall".ToLower()))
        {
            other.gameObject.transform.SetLocalPositionAndRotation(new Vector3(-1.36000001f, 0.0587999991f, 0.324745178f), new Quaternion(-0.707106829f, 0f, 0f, 0.707106709f));
            Rigidbody rb = other.GetComponent<Rigidbody>();
            other.attachedRigidbody.linearVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
