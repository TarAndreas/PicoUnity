using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBall : MonoBehaviour {

    protected Vector3 Startpos;
    protected bool ResetIt;
    Vector3 lastVelocity;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        Startpos = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        var speed = lastVelocity.magnitude;
        if (speed <= 0)
        {
            speed = 0.05f;
        }
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.linearVelocity = direction * Mathf.Max(speed, 0f);
    }
    // Update is called once per frame
    void Update () {
        lastVelocity = rb.linearVelocity;
        if(this.transform.position == new Vector3(-1.36000001f, 0.0587999991f, 0.324745178f))
        {
            lastVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
        //if (transform.position.y < -0.1f)
        //{
        //    transform.position = new Vector3(0, 0.08f, 0);
        //    GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}

        //if (ResetIt)
        //{
        //    ResetIt = false;
        //    transform.position = Startpos;
        //    GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}
    }
    public void ResetBall()
    {
        ResetIt = true;
    }
}
