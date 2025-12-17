/*
 * Assign GameObject to activate via script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{

    public GameObject toActivate;

    // Start is called before the first frame update
    void Start()
    {
        toActivate.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
