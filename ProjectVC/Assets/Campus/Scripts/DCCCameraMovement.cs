/*
 * Camera position equal to attached GameObject
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCCCameraMovement : MonoBehaviour
{
    public Transform campos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = campos.position;
    }
}
