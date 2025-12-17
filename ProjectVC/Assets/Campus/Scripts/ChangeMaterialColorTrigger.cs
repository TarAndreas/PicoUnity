using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialColorTrigger : MonoBehaviour
{
    
    [SerializeField]
    public GameObject currentObject;
    

    [SerializeField]
    public string tagname;

    [SerializeField]
    public Material newMaterial;


    public void changeMaterial()
    {
        Renderer rend = currentObject.GetComponent<Renderer>();
        rend.material = newMaterial;
        
    }

    public void OnTriggerEnter(Collider other)
    {
        /*
        if (other.gameObject.tag == tagname)
        {
            changeMaterial();
        }
        */

        changeMaterial();
    }

 }
