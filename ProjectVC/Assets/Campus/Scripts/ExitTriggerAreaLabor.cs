/*
 * Set active or inactive Gameobject once trigger box left or entered
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTriggerAreaLabor : MonoBehaviour

{
    [SerializeField]
    public GameObject ActivateObject;

    [SerializeField]
    public GameObject DeactivateObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerExit(Collider other)
    {
        if(other.name.Contains("VRPlayerPrefab"))
        {
            ActivateObject.SetActive(true);
            DeactivateObject.SetActive(false);
            Debug.Log("Leaving Trigger Area/Room");
        } else
        {
            Debug.Log("Player still in area");

        }



    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("VRPlayerPrefab"))
        {
            ActivateObject.SetActive(false);
            DeactivateObject.SetActive(true);
            Debug.Log("Player in area again");
        }
        else
        {
            Debug.Log("Player still in area");
        }
    }
    
}
