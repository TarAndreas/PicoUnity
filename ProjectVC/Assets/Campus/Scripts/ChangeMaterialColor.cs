/*
 * Change color of the objects rederer
 * Invoke once the particle collision hits a certain tag object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    
    [SerializeField]
    public GameObject currentObject;
    

    [SerializeField]
    public string tagname;

    [SerializeField]
    public Material newMaterial;


    public void changeColor()
    {
        var objectRenderer = currentObject.GetComponent<Renderer>();
        objectRenderer.material.SetColor("_Color", Color.red);
    }

    public void changeMaterial()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material = newMaterial;
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.tag == tagname)
        {
            changeMaterial();
        }
    }
 }
