/*
 * Trigger regulate what GameObjects are activated or set inactive
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingTriggerArea : MonoBehaviour
{
    //GameObject that will be deactivated
    public GameObject deactivateGameObject;
    //GameObject that will be activated
    public GameObject activateGameObject;
    public GameObject activateSecondGameObject;
    public GameObject checkActivity;
    private bool isDeactivated = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Object")
        {
            deactivateGameObject.SetActive(true);
            isDeactivated = false;
            ActivateGO();
            CheckForActivity();
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
            deactivateGameObject.SetActive(false);
            isDeactivated = true;
            ActivateGO();
            CheckForActivity();
        
    }

    public void ActivateGO()
    {
        if (isDeactivated == true) {
            activateGameObject.SetActive(true);
            activateSecondGameObject.SetActive(false);
            
        } else
        {
            activateGameObject.SetActive(false);
            
        }
    }

    //Checks if the GameObject is active
    public void CheckForActivity()
    {

        if(checkActivity.activeSelf)
        {
            activateSecondGameObject.SetActive(true);
            
        }
    }
}
