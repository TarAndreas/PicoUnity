using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables axis rotation on a GameObject
/// </summary>
public class RotationAxis : MonoBehaviour
{


    [SerializeField] private Vector3 rotation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
        

    }
}
