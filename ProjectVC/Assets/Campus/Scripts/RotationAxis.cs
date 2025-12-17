using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates GameObject on Z axis
/// </summary>

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 5 * Time.deltaTime);
    }
}
