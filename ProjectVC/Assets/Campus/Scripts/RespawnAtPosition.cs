using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resets position and rotation of assigned GameObject to given coordinates
/// </summary>

public class RespawnAtPosition : MonoBehaviour
{

    Vector3 startPosition;
    Quaternion startRotation;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void RespawnObject()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
