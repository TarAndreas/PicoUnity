using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBall : MonoBehaviour {

    protected Vector3 Startpos;
    protected bool ResetIt;
    Vector3 lastVelocity;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        Startpos = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        lastVelocity = rb.linearVelocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        var speed = lastVelocity.magnitude;
        if(speed <= 0)
        {
            speed = 0.05f;
        }
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.linearVelocity = direction * Mathf.Max(speed, 0f);
    }
    // Update is called once per frame
    void FixedUpdate () {

        if (transform.position.y < 0.01f)
            gameObject.SetActive(false);

        if (ResetIt)
        {
            ResetIt = false;
            transform.position = Startpos;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
	}

    public void ResetBall()
    {
        ResetIt = true;
    }
}
